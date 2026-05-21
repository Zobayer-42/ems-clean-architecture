using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Application.DTO.Request.Employee;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Route("api/employees")]
[Authorize]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    [HasPermission("employee.read")]
    public async Task<IActionResult> GetAll()
    {
        var response = await _employeeService.GetAllAsync();
        return StatusCode(int.Parse(response.StatusCode), response);
    }

    [HttpGet("{id:guid}")]
    [HasPermission("employee.read")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _employeeService.GetByIdAsync(id);
        return StatusCode(int.Parse(response.StatusCode), response);
    }

    [HttpPost]
    [HasPermission("employee.create")]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request)
    {
        var response = await _employeeService.CreateAsync(request);
        return StatusCode(int.Parse(response.StatusCode), response);
    }

    [HttpPut("{id:guid}")]
    [HasPermission("employee.update")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeRequest request)
    {
        var response = await _employeeService.UpdateAsync(id, request);
        return StatusCode(int.Parse(response.StatusCode), response);
    }

    [HttpPatch("{id:guid}/deactivate")]
    [HasPermission("employee.delete")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var response = await _employeeService.DeactivateAsync(id);
        return StatusCode(int.Parse(response.StatusCode), response);
    }
}