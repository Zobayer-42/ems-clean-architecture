using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Application.Contracts;

public interface IJwtTokenService
{
    string GenerateToken(User user, List<string> roles, List<string> permissions);
    Guid? GetUserIdFromToken(string token);
}