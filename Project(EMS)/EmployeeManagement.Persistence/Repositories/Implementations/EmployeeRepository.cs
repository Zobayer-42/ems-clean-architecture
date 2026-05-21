using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.DbContext;
using EmployeeManagement.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Persistence.Repositories.Implementations;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(AppDbContext context) : base(context) { }
    public async Task<Employee?> GetWithDetailsAsync(Guid id)
        => await _dbSet
            .Include(x => x.Department)
            .Include(x => x.Designation)
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<List<Employee>> GetAllWithDetailsAsync()
        => await _dbSet
            .Include(x => x.Department)
            .Include(x => x.Designation)
            .ToListAsync();

    public async Task<bool> IsEmailExistsAsync(string email, Guid? excludeId = null)
        => await _dbSet.AnyAsync(x =>
            x.Email == email &&
            (excludeId == null || x.Id != excludeId));
}