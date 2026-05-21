using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Application.DTO.Request.Auth;
using EmployeeManagement.Application.DTO.Response.Auth;
using EmployeeManagement.Shared.Response;

namespace EmployeeManagement.Application.Services.Interfaces;

public interface IAuthService
{
    Task<StandardResponse<AuthResponse>> LoginAsync(LoginRequest request);
    Task<StandardResponse<bool>> LogoutAsync(Guid userId);
    Task<StandardResponse<bool>> RegisterAsync(RegisterRequest request);
}
