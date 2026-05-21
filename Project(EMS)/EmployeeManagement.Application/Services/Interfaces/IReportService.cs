using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Application.DTO.Response.Salary;
using EmployeeManagement.Shared.Response;

namespace EmployeeManagement.Application.Services.Interfaces;

public interface IReportService
{
    Task<StandardResponse<decimal>> GetTotalDisbursedAsync(int month, int year);
    Task<StandardResponse<List<SalaryResponse>>> GetSalaryByDepartmentAsync(Guid departmentId);
    Task<StandardResponse<List<SalaryResponse>>> GetMonthlySummaryAsync(int month, int year);
    Task<StandardResponse<List<SalaryResponse>>> GetEmployeeSalaryHistoryAsync(Guid employeeId);
}