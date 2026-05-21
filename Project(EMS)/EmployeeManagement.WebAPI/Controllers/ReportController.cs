using Microsoft.AspNetCore.Http;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("total-disbursed/{month:int}/{year:int}")]
    [HasPermission("report.read")]
    public async Task<IActionResult> GetTotalDisbursed(int month, int year)
    {
        var response = await _reportService.GetTotalDisbursedAsync(month, year);
        return StatusCode(int.Parse(response.StatusCode), response);
    }

    [HttpGet("department/{departmentId:guid}")]
    [HasPermission("report.read")]
    public async Task<IActionResult> GetByDepartment(Guid departmentId)
    {
        var response = await _reportService.GetSalaryByDepartmentAsync(departmentId);
        return StatusCode(int.Parse(response.StatusCode), response);
    }

    [HttpGet("monthly-summary/{month:int}/{year:int}")]
    [HasPermission("report.read")]
    public async Task<IActionResult> GetMonthlySummary(int month, int year)
    {
        var response = await _reportService.GetMonthlySummaryAsync(month, year);
        return StatusCode(int.Parse(response.StatusCode), response);
    }

    [HttpGet("employee-history/{employeeId:guid}")]
    [HasPermission("report.read")]
    public async Task<IActionResult> GetEmployeeHistory(Guid employeeId)
    {
        var response = await _reportService.GetEmployeeSalaryHistoryAsync(employeeId);
        return StatusCode(int.Parse(response.StatusCode), response);
    }
}