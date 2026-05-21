using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Application.DTO.Response.Salary;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Persistence.UnitOfWork;
using EmployeeManagement.Shared.Exceptions;
using EmployeeManagement.Shared.Response;

namespace EmployeeManagement.Application.Services.Implementations;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReportService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<StandardResponse<decimal>> GetTotalDisbursedAsync(int month, int year)
    {
        var total = await _unitOfWork.Salaries.GetTotalDisbursedAsync(month, year);

        return ResponseHelper.Success(total,
            $"Total salary disbursed for {month}/{year}: {total:C}");
    }

    public async Task<StandardResponse<List<SalaryResponse>>> GetSalaryByDepartmentAsync(Guid departmentId)
    {
        _ = await _unitOfWork.Departments.GetByIdAsync(departmentId)
            ?? throw new NotFoundException(nameof(Department), departmentId);

        var salaries = await _unitOfWork.Salaries
            .FindAsync(s => s.Employee.DepartmentId == departmentId);

        var response = salaries.Select(s => MapToResponse(s)).ToList();

        return ResponseHelper.Success(response,
            $"{response.Count} salary records for department.");
    }

    public async Task<StandardResponse<List<SalaryResponse>>> GetMonthlySummaryAsync(int month, int year)
    {
        var salaries = await _unitOfWork.Salaries.GetByMonthYearAsync(month, year);
        var response = salaries.Select(s => MapToResponse(s)).ToList();

        return ResponseHelper.Success(response,
            $"Monthly summary for {month}/{year}: {response.Count} records.");
    }

    public async Task<StandardResponse<List<SalaryResponse>>> GetEmployeeSalaryHistoryAsync(Guid employeeId)
    {
        _ = await _unitOfWork.Employees.GetByIdAsync(employeeId)
            ?? throw new NotFoundException(nameof(Employee), employeeId);

        var salaries = await _unitOfWork.Salaries.GetByEmployeeAsync(employeeId);
        var response = salaries.Select(s => MapToResponse(s)).ToList();

        return ResponseHelper.Success(response,
            $"{response.Count} salary history records retrieved.");
    }
    private static SalaryResponse MapToResponse(SalaryDisbursement s) => new()
    {
        Id = s.Id,
        EmployeeId = s.EmployeeId,
        EmployeeName = $"{s.Employee?.FirstName} {s.Employee?.LastName}",
        DepartmentName = s.Employee?.Department?.Name ?? "",
        Month = s.Month,
        Year = s.Year,
        BasicSalary = s.BasicSalary,
        Bonus = s.Bonus,
        Deduction = s.Deduction,
        NetSalary = s.NetSalary,
        Status = s.Status.ToString(),
        DisbursedAt = s.DisbursedAt,
        Remarks = s.Remarks
    };
}