using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Application.DTO.Request.Salary;
using EmployeeManagement.Application.DTO.Response.Salary;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Domain.Enums;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Application.Contracts;
using EmployeeManagement.Persistence.UnitOfWork;
using EmployeeManagement.Shared.Exceptions;
using EmployeeManagement.Shared.Response;

namespace EmployeeManagement.Application.Services.Implementations;

public class SalaryService : ISalaryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogService _logService;

    public SalaryService(IUnitOfWork unitOfWork, ILogService logService)
    {
        _unitOfWork = unitOfWork;
        _logService = logService;
    }

    public async Task<StandardResponse<SalaryResponse>> DisburseAsync(SalaryDisbursementRequest request)
    {
        var employee = await _unitOfWork.Employees.GetWithDetailsAsync(request.EmployeeId)
            ?? throw new NotFoundException(nameof(Employee), request.EmployeeId);

        if (employee.Status != EmployeeStatus.Active)
            return ResponseHelper.Fail<SalaryResponse>(
                "Salary can only be disbursed for active employees.", "400");

        var alreadyDisbursed = await _unitOfWork.Salaries
            .IsAlreadyDisbursedAsync(request.EmployeeId, request.Month, request.Year);

        if (alreadyDisbursed)
            return ResponseHelper.Fail<SalaryResponse>(
                $"Salary for {request.Month}/{request.Year} has already been disbursed.", "409");

        var netSalary = employee.BasicSalary + request.Bonus - request.Deduction;

        if (netSalary < 0)
            return ResponseHelper.Fail<SalaryResponse>(
                "Deduction cannot exceed basic salary plus bonus.", "400");

        var disbursement = new SalaryDisbursement
        {
            EmployeeId = request.EmployeeId,
            Month = request.Month,
            Year = request.Year,
            BasicSalary = employee.BasicSalary,
            Bonus = request.Bonus,
            Deduction = request.Deduction,
            NetSalary = netSalary,
            DisbursedAt = DateTime.UtcNow,
            Status = PaymentStatus.Paid,
            Remarks = request.Remarks
        };

        await _unitOfWork.Salaries.AddAsync(disbursement);
        await _unitOfWork.SaveChangesAsync();

        await _logService.LogCreateAsync("Salary", disbursement.Id,
            $"Salary disbursed for employee {employee.FirstName} {employee.LastName} " +
            $"— {request.Month}/{request.Year} — Net: {netSalary}");

        return ResponseHelper.Success(
            MapToResponse(disbursement, employee),
            "Salary disbursed successfully.", "201");
    }

    public async Task<StandardResponse<List<SalaryResponse>>> GetByEmployeeAsync(Guid employeeId)
    {
        _ = await _unitOfWork.Employees.GetByIdAsync(employeeId)
            ?? throw new NotFoundException(nameof(Employee), employeeId);

        var salaries = await _unitOfWork.Salaries.GetByEmployeeAsync(employeeId);
        var response = salaries.Select(s => MapToResponse(s, s.Employee)).ToList();

        return ResponseHelper.Success(response, $"{response.Count} salary records retrieved.");
    }

    public async Task<StandardResponse<List<SalaryResponse>>> GetByMonthYearAsync(int month, int year)
    {
        var salaries = await _unitOfWork.Salaries.GetByMonthYearAsync(month, year);
        var response = salaries.Select(s => MapToResponse(s, s.Employee)).ToList();

        return ResponseHelper.Success(response,
            $"{response.Count} salary records retrieved for {month}/{year}.");
    }
    private static SalaryResponse MapToResponse(SalaryDisbursement s, Employee e) => new()
    {
        Id = s.Id,
        EmployeeId = s.EmployeeId,
        EmployeeName = $"{e.FirstName} {e.LastName}",
        DepartmentName = e.Department?.Name ?? "",
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
