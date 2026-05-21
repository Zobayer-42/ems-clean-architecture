using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Application.Contracts;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace EmployeeManagement.Infrastructure.Logging;

public class LogService : ILogService
{
    private readonly LogDbContext _logDb;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LogService(LogDbContext logDb, IHttpContextAccessor httpContextAccessor)
    {
        _logDb = logDb;
        _httpContextAccessor = httpContextAccessor;
    }

    private string? GetCurrentUserId()
        => _httpContextAccessor.HttpContext?.User
            .FindFirstValue(ClaimTypes.NameIdentifier);

    private string? GetCurrentUsername()
        => _httpContextAccessor.HttpContext?.User
            .FindFirstValue(ClaimTypes.Name);

    private string? GetIpAddress()
        => _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

    public async Task LogAsync(
        string action, string module, bool isSuccess = true,
        string? details = null, string? errorMessage = null)
    {
        var log = new ActivityLog
        {
            Action = action,
            Module = module,
            UserId = GetCurrentUserId(),
            Username = GetCurrentUsername(),
            IpAddress = GetIpAddress(),
            Details = details,
            IsSuccess = isSuccess,
            ErrorMessage = errorMessage,
            CreatedAt = DateTime.UtcNow
        };

        await _logDb.ActivityLogs.AddAsync(log);
        await _logDb.SaveChangesAsync();
    }

    public async Task LogLoginAsync(string username, bool isSuccess, string? ipAddress = null)
    {
        var log = new ActivityLog
        {
            Action = "Login",
            Module = "Auth",
            Username = username,
            IpAddress = ipAddress ?? GetIpAddress(),
            IsSuccess = isSuccess,
            ErrorMessage = isSuccess ? null : "Invalid credentials",
            CreatedAt = DateTime.UtcNow
        };

        await _logDb.ActivityLogs.AddAsync(log);
        await _logDb.SaveChangesAsync();
    }

    public async Task LogLogoutAsync(string username)
        => await LogAsync("Logout", "Auth", details: $"User '{username}' logged out");

    public async Task LogCreateAsync(string module, Guid entityId, string? details = null)
        => await LogAsync(
            "Create", module,
            details: details ?? JsonSerializer.Serialize(new { entityId }));

    public async Task LogUpdateAsync(string module, Guid entityId, string? details = null)
        => await LogAsync(
            "Update", module,
            details: details ?? JsonSerializer.Serialize(new { entityId }));

    public async Task LogDeleteAsync(string module, Guid entityId)
        => await LogAsync(
            "Delete", module,
            details: JsonSerializer.Serialize(new { entityId }));

    public async Task LogErrorAsync(string module, string errorMessage, string? details = null)
        => await LogAsync(
            "Error", module,
            isSuccess: false,
            details: details,
            errorMessage: errorMessage);
}