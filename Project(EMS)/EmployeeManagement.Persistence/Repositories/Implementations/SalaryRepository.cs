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

public class SalaryRepository : Repository<SalaryDisbursement>, ISalaryRepository
{
    public SalaryRepository(AppDbContext context) : base(context) { }

    public async Task<List<SalaryDisbursement>> GetByEmployeeAsync(Guid employeeId)
        => await _dbSet
            .Include(x => x.Employee)
            .ThenInclude(x => x.Department)
            .Where(x => x.EmployeeId == employeeId)
            .OrderByDescending(x => x.Year)
            .ThenByDescending(x => x.Month)
            .ToListAsync();

    public async Task<List<SalaryDisbursement>> GetByMonthYearAsync(int month, int year)
        => await _dbSet
            .Include(x => x.Employee)
            .ThenInclude(x => x.Department)
            .Where(x => x.Month == month && x.Year == year)
            .ToListAsync();

    public async Task<bool> IsAlreadyDisbursedAsync(Guid employeeId, int month, int year)
        => await _dbSet.AnyAsync(x =>
            x.EmployeeId == employeeId &&
            x.Month == month &&
            x.Year == year);

    public async Task<decimal> GetTotalDisbursedAsync(int month, int year)
        => await _dbSet
            .Where(x => x.Month == month && x.Year == year)
            .SumAsync(x => x.NetSalary);
}