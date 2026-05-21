using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Application.DTO.Request.Salary;
using EmployeeManagement.Application.DTO.Response.Salary;
using EmployeeManagement.Shared.Response;

namespace EmployeeManagement.Application.Services.Interfaces;

public interface ISalaryService
{
    Task<StandardResponse<SalaryResponse>> DisburseAsync(SalaryDisbursementRequest request);
    Task<StandardResponse<List<SalaryResponse>>> GetByEmployeeAsync(Guid employeeId);
    Task<StandardResponse<List<SalaryResponse>>> GetByMonthYearAsync(int month, int year);
}