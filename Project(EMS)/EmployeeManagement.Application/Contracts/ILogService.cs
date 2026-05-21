using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EmployeeManagement.Application.Contracts;

public interface ILogService
{
    Task LogAsync(string action, string module, bool isSuccess = true,
        string? details = null, string? errorMessage = null);
    Task LogLoginAsync(string username, bool isSuccess, string? ipAddress = null);
    Task LogLogoutAsync(string username);
    Task LogCreateAsync(string module, Guid entityId, string? details = null);
    Task LogUpdateAsync(string module, Guid entityId, string? details = null);
    Task LogDeleteAsync(string module, Guid entityId);
    Task LogErrorAsync(string module, string errorMessage, string? details = null);
}