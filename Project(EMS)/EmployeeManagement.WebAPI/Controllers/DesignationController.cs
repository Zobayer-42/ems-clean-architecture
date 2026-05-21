using EmployeeManagement.Application.DTO.Request.Designation;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Route("api/designations")]
[Authorize]
public class DesignationController : ControllerBase
{
    private readonly IDesignationService _designationService;

    public DesignationController(IDesignationService designationService)
    {
        _designationService = designationService;
    }

    [HttpGet]
    [HasPermission("department.read")]
    public async Task<IActionResult> GetAll()
    {
        var response = await _designationService.GetAllAsync();
        return StatusCode(int.Parse(response.StatusCode), response);
    }

    [HttpGet("{id:guid}")]
    [HasPermission("department.read")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await _designationService.GetByIdAsync(id);
        return StatusCode(int.Parse(response.StatusCode), response);
    }

    [HttpPost]
    [HasPermission("department.create")]
    public async Task<IActionResult> Create([FromBody] CreateDesignationRequest request)
    {
        var response = await _designationService.CreateAsync(request);
        return StatusCode(int.Parse(response.StatusCode), response);
    }

    [HttpPut("{id:guid}")]
    [HasPermission("department.update")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDesignationRequest request)
    {
        var response = await _designationService.UpdateAsync(id, request);
        return StatusCode(int.Parse(response.StatusCode), response);
    }

    [HttpDelete("{id:guid}")]
    [HasPermission("department.delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _designationService.DeleteAsync(id);
        return StatusCode(int.Parse(response.StatusCode), response);
    }
}