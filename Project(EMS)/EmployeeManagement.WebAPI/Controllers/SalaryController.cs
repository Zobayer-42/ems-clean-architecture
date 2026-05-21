using Microsoft.AspNetCore.Http;
using EmployeeManagement.Application.DTO.Request.Salary;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Route("api/salaries")]
[Authorize]
public class SalaryController : ControllerBase
{
    private readonly ISalaryService _salaryService;

    public SalaryController(ISalaryService salaryService)
    {
        _salaryService = salaryService;
    }

    [HttpPost("disburse")]
    [HasPermission("salary.disburse")]
    public async Task<IActionResult> Disburse([FromBody] SalaryDisbursementRequest request)
    {
        var response = await _salaryService.DisburseAsync(request);
        return StatusCode(int.Parse(response.StatusCode), response);
    }

    [HttpGet("employee/{employeeId:guid}")]
    [HasPermission("salary.read")]
    public async Task<IActionResult> GetByEmployee(Guid employeeId)
    {
        var response = await _salaryService.GetByEmployeeAsync(employeeId);
        return StatusCode(int.Parse(response.StatusCode), response);
    }

    [HttpGet("{month:int}/{year:int}")]
    [HasPermission("salary.read")]
    public async Task<IActionResult> GetByMonthYear(int month, int year)
    {
        var response = await _salaryService.GetByMonthYearAsync(month, year);
        return StatusCode(int.Parse(response.StatusCode), response);
    }
}