using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Application.DTO.Request.Department;
using EmployeeManagement.Application.DTO.Response.Department;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Application.Contracts;
using EmployeeManagement.Persistence.UnitOfWork;
using EmployeeManagement.Shared.Exceptions;
using EmployeeManagement.Shared.Response;

namespace EmployeeManagement.Application.Services.Implementations;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogService _logService;

    public DepartmentService(IUnitOfWork unitOfWork, ILogService logService)
    {
        _unitOfWork = unitOfWork;
        _logService = logService;
    }

    public async Task<StandardResponse<DepartmentResponse>> CreateAsync(CreateDepartmentRequest request)
    {
        var exists = await _unitOfWork.Departments
            .ExistsAsync(d => d.Name == request.Name);

        if (exists)
            return ResponseHelper.Fail<DepartmentResponse>(
                "Department with this name already exists.", "409");

        var department = new Department
        {
            Name = request.Name,
            Description = request.Description
        };

        await _unitOfWork.Departments.AddAsync(department);
        await _unitOfWork.SaveChangesAsync();

        await _logService.LogCreateAsync("Department", department.Id);

        return ResponseHelper.Success(
            MapToResponse(department), "Department created successfully.", "201");
    }

    public async Task<StandardResponse<DepartmentResponse>> UpdateAsync(Guid id, UpdateDepartmentRequest request)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Department), id);
        var exists = await _unitOfWork.Departments
            .ExistsAsync(d => d.Name == request.Name && d.Id != id);

        if (exists)
            return ResponseHelper.Fail<DepartmentResponse>(
                "Department with this name already exists.", "409");

        department.Name = request.Name;
        department.Description = request.Description;
        department.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Departments.Update(department);
        await _unitOfWork.SaveChangesAsync();

        await _logService.LogUpdateAsync("Department", department.Id);

        return ResponseHelper.Success(MapToResponse(department), "Department updated successfully.");
    }

    public async Task<StandardResponse<DepartmentResponse>> GetByIdAsync(Guid id)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Department), id);

        return ResponseHelper.Success(MapToResponse(department));
    }

    public async Task<StandardResponse<List<DepartmentResponse>>> GetAllAsync()
    {
        var departments = await _unitOfWork.Departments.GetAllAsync();
        var response = departments.Select(MapToResponse).ToList();

        return ResponseHelper.Success(response, $"{response.Count} departments retrieved.");
    }

    public async Task<StandardResponse<bool>> DeleteAsync(Guid id)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Department), id);
        var hasEmployees = await _unitOfWork.Employees
            .ExistsAsync(e => e.DepartmentId == id);

        if (hasEmployees)
            return ResponseHelper.Fail<bool>(
                "Cannot delete department with active employees.", "400");

        _unitOfWork.Departments.Delete(department);
        await _unitOfWork.SaveChangesAsync();

        await _logService.LogDeleteAsync("Department", id);

        return ResponseHelper.Success(true, "Department deleted successfully.");
    }
    private static DepartmentResponse MapToResponse(Department d) => new()
    {
        Id = d.Id,
        Name = d.Name,
        Description = d.Description,
        CreatedAt = d.CreatedAt
    };
}
