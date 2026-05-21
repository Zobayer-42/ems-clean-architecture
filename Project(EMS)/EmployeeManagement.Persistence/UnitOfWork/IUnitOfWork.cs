using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.Repositories.Interfaces;

namespace EmployeeManagement.Persistence.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IEmployeeRepository Employees { get; }
    ISalaryRepository Salaries { get; }
    IRepository<Department> Departments { get; }
    IRepository<Designation> Designations { get; }
    IRepository<User> Users { get; }
    IRepository<Role> Roles { get; }
    IRepository<Permission> Permissions { get; }
    IRepository<UserRole> UserRoles { get; }           
    IRepository<RolePermission> RolePermissions { get; } 

    Task<int> SaveChangesAsync();
}