using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Application.DTO.Request.Auth;
using EmployeeManagement.Application.DTO.Response.Auth;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Application.Contracts;
using EmployeeManagement.Persistence.UnitOfWork;
using EmployeeManagement.Shared.Exceptions;
using EmployeeManagement.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Application.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogService _logService;

    public AuthService(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        ILogService logService)
    {
        _unitOfWork      = unitOfWork;
        _passwordHasher  = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _logService      = logService;
    }

    public async Task<StandardResponse<AuthResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _unitOfWork.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email && !u.IsDeleted);

        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            await _logService.LogLoginAsync(request.Email, isSuccess: false);
            throw new UnauthorizedException("Invalid email or password.");
        }

        if (!user.IsActive)
        {
            await _logService.LogLoginAsync(request.Email, isSuccess: false);
            throw new UnauthorizedException("Your account has been deactivated.");
        }

        var roles       = await GetUserRolesAsync(user.Id);
        var permissions = await GetUserPermissionsAsync(user.Id);

        var token = _jwtTokenService.GenerateToken(user, roles, permissions);

        await _logService.LogLoginAsync(user.Username, isSuccess: true);

        return ResponseHelper.Success(new AuthResponse
        {
            Token       = token,
            Username    = user.Username,
            Email       = user.Email,
            Roles       = roles,
            Permissions = permissions,
            ExpiresAt   = DateTime.UtcNow.AddMinutes(60)
        }, "Login successful");
    }

    public async Task<StandardResponse<bool>> LogoutAsync(Guid userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId)
            ?? throw new NotFoundException(nameof(User), userId);

        await _logService.LogLogoutAsync(user.Username);

        return ResponseHelper.Success(true, "Logged out successfully.");
    }

    public async Task<StandardResponse<bool>> RegisterAsync(RegisterRequest request)
    {
        var exists = await _unitOfWork.Users
            .ExistsAsync(u => u.Email == request.Email);

        if (exists)
            return ResponseHelper.Fail<bool>("Email already in use.", "409");

        var user = new User
        {
            Username     = request.Username,
            Email        = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password),
            IsActive     = true
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        await _logService.LogCreateAsync("Auth", user.Id, $"User '{user.Username}' registered.");

        return ResponseHelper.Success(true, "Registration successful.", "201");
    }
    private async Task<List<string>> GetUserRolesAsync(Guid userId)
    {
        var userRoles = await _unitOfWork.UserRoles
            .FindAsync(ur => ur.UserId == userId);

        var roles = new List<string>();
        foreach (var ur in userRoles)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(ur.RoleId);
            if (role is not null)
                roles.Add(role.Name);
        }

        return roles;
    }

    private async Task<List<string>> GetUserPermissionsAsync(Guid userId)
    {
        var userRoles = await _unitOfWork.UserRoles
            .FindAsync(ur => ur.UserId == userId);

        var permissions = new List<string>();
        foreach (var ur in userRoles)
        {
            var rolePermissions = await _unitOfWork.RolePermissions
                .FindAsync(rp => rp.RoleId == ur.RoleId);

            foreach (var rp in rolePermissions)
            {
                var permission = await _unitOfWork.Permissions.GetByIdAsync(rp.PermissionId);
                if (permission is not null && !permissions.Contains(permission.Name))
                    permissions.Add(permission.Name);
            }
        }

        return permissions;
    }
}
