using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Application.DTO.Request.Designation;
using EmployeeManagement.Application.DTO.Response.Designation;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Application.Contracts;
using EmployeeManagement.Persistence.UnitOfWork;
using EmployeeManagement.Shared.Exceptions;
using EmployeeManagement.Shared.Response;

namespace EmployeeManagement.Application.Services.Implementations;

public class DesignationService : IDesignationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogService _logService;

    public DesignationService(IUnitOfWork unitOfWork, ILogService logService)
    {
        _unitOfWork = unitOfWork;
        _logService = logService;
    }

    public async Task<StandardResponse<DesignationResponse>> CreateAsync(CreateDesignationRequest request)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId)
            ?? throw new NotFoundException(nameof(Department), request.DepartmentId);

        var exists = await _unitOfWork.Designations
            .ExistsAsync(d => d.Title == request.Title && d.DepartmentId == request.DepartmentId);

        if (exists)
            return ResponseHelper.Fail<DesignationResponse>(
                "Designation with this title already exists in the department.", "409");

        var designation = new Designation
        {
            Title = request.Title,
            Description = request.Description,
            DepartmentId = request.DepartmentId
        };

        await _unitOfWork.Designations.AddAsync(designation);
        await _unitOfWork.SaveChangesAsync();

        await _logService.LogCreateAsync("Designation", designation.Id);

        return ResponseHelper.Success(
            MapToResponse(designation, department.Name),
            "Designation created successfully.", "201");
    }

    public async Task<StandardResponse<DesignationResponse>> UpdateAsync(Guid id, UpdateDesignationRequest request)
    {
        var designation = await _unitOfWork.Designations.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Designation), id);

        var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId)
            ?? throw new NotFoundException(nameof(Department), request.DepartmentId);

        designation.Title = request.Title;
        designation.Description = request.Description;
        designation.DepartmentId = request.DepartmentId;
        designation.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Designations.Update(designation);
        await _unitOfWork.SaveChangesAsync();

        await _logService.LogUpdateAsync("Designation", designation.Id);

        return ResponseHelper.Success(
            MapToResponse(designation, department.Name), "Designation updated successfully.");
    }

    public async Task<StandardResponse<DesignationResponse>> GetByIdAsync(Guid id)
    {
        var designation = await _unitOfWork.Designations.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Designation), id);

        var department = await _unitOfWork.Departments.GetByIdAsync(designation.DepartmentId);

        return ResponseHelper.Success(
            MapToResponse(designation, department?.Name ?? ""));
    }

    public async Task<StandardResponse<List<DesignationResponse>>> GetAllAsync()
    {
        var designations = await _unitOfWork.Designations.GetAllAsync();
        var result = new List<DesignationResponse>();

        foreach (var d in designations)
        {
            var dept = await _unitOfWork.Departments.GetByIdAsync(d.DepartmentId);
            result.Add(MapToResponse(d, dept?.Name ?? ""));
        }

        return ResponseHelper.Success(result, $"{result.Count} designations retrieved.");
    }

    public async Task<StandardResponse<bool>> DeleteAsync(Guid id)
    {
        var designation = await _unitOfWork.Designations.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Designation), id);

        var hasEmployees = await _unitOfWork.Employees
            .ExistsAsync(e => e.DesignationId == id);

        if (hasEmployees)
            return ResponseHelper.Fail<bool>(
                "Cannot delete designation with active employees.", "400");

        _unitOfWork.Designations.Delete(designation);
        await _unitOfWork.SaveChangesAsync();

        await _logService.LogDeleteAsync("Designation", id);

        return ResponseHelper.Success(true, "Designation deleted successfully.");
    }
    private static DesignationResponse MapToResponse(Designation d, string departmentName) => new()
    {
        Id = d.Id,
        Title = d.Title,
        Description = d.Description,
        DepartmentId = d.DepartmentId,
        DepartmentName = departmentName,
        CreatedAt = d.CreatedAt
    };
}
