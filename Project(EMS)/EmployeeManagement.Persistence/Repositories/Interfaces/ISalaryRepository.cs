using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Persistence.Repositories.Interfaces;

public interface ISalaryRepository : IRepository<SalaryDisbursement>
{
    Task<List<SalaryDisbursement>> GetByEmployeeAsync(Guid employeeId);
    Task<List<SalaryDisbursement>> GetByMonthYearAsync(int month, int year);
    Task<bool> IsAlreadyDisbursedAsync(Guid employeeId, int month, int year);
    Task<decimal> GetTotalDisbursedAsync(int month, int year);
}