using Microsoft.AspNetCore.Http;
using EmployeeManagement.Application.DTO.Request.Department;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Route("api/departments")]
[Authorize]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    [HasPermission("department.read")]
    public async Task<IActionResult> GetAll()
    {
        var response = await _departmentService.GetAllAsync();
        return StatusCode(int.Parse(response.StatusCode), response);
    }

    [HttpGet("{id:guid}")]
    [HasPermission("department.read")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _departmentService.GetByIdAsync(id);
        return StatusCode(int.Parse(response.StatusCode), response);
    }

    [HttpPost]
    [HasPermission("department.create")]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentRequest request)
    {
        var response = await _departmentService.CreateAsync(request);
        return StatusCode(int.Parse(response.StatusCode), response);
    }

    [HttpPut("{id:guid}")]
    [HasPermission("department.update")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDepartmentRequest request)
    {
        var response = await _departmentService.UpdateAsync(id, request);
        return StatusCode(int.Parse(response.StatusCode), response);
    }

    [HttpDelete("{id:guid}")]
    [HasPermission("department.delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _departmentService.DeleteAsync(id);
        return StatusCode(int.Parse(response.StatusCode), response);
    }
}