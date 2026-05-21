using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.DbContext;
using EmployeeManagement.Persistence.Repositories.Implementations;
using EmployeeManagement.Persistence.Repositories.Interfaces;

namespace EmployeeManagement.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IEmployeeRepository Employees { get; }
    public ISalaryRepository Salaries { get; }
    public IRepository<Department> Departments { get; }
    public IRepository<Designation> Designations { get; }
    public IRepository<User> Users { get; }
    public IRepository<Role> Roles { get; }
    public IRepository<Permission> Permissions { get; }
    public IRepository<UserRole> UserRoles { get; }           // ← এটা যোগ করো
    public IRepository<RolePermission> RolePermissions { get; } // ← এটা যোগ করো

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Employees = new EmployeeRepository(context);
        Salaries = new SalaryRepository(context);
        Departments = new Repository<Department>(context);
        Designations = new Repository<Designation>(context);
        Users = new Repository<User>(context);
        Roles = new Repository<Role>(context);
        Permissions = new Repository<Permission>(context);
        UserRoles = new Repository<UserRole>(context);     
        RolePermissions = new Repository<RolePermission>(context); 
    }

    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public void Dispose()
        => _context.Dispose();
}