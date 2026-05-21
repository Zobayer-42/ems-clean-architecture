using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Application.DTO.Request.Employee;
using EmployeeManagement.Application.DTO.Response.Employee;
using EmployeeManagement.Application.Services.Interfaces;
using EmployeeManagement.Domain.Enums;
using EmployeeManagement.Domain.Models;
using EmployeeManagement.Application.Contracts;
using EmployeeManagement.Persistence.UnitOfWork;
using EmployeeManagement.Shared.Exceptions;
using EmployeeManagement.Shared.Response;

namespace EmployeeManagement.Application.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogService _logService;

    public EmployeeService(IUnitOfWork unitOfWork, ILogService logService)
    {
        _unitOfWork = unitOfWork;
        _logService = logService;
    }

    public async Task<StandardResponse<EmployeeResponse>> CreateAsync(CreateEmployeeRequest request)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId)
            ?? throw new NotFoundException(nameof(Department), request.DepartmentId);

        var designation = await _unitOfWork.Designations.GetByIdAsync(request.DesignationId)
            ?? throw new NotFoundException(nameof(Designation), request.DesignationId);
        var emailExists = await _unitOfWork.Employees
            .IsEmailExistsAsync(request.Email);

        if (emailExists)
            return ResponseHelper.Fail<EmployeeResponse>(
                "Employee with this email already exists.", "409");

        var employee = new Employee
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Phone = request.Phone,
            DateOfBirth = request.DateOfBirth,
            JoiningDate = request.JoiningDate,
            BasicSalary = request.BasicSalary,
            DepartmentId = request.DepartmentId,
            DesignationId = request.DesignationId,
            Status = EmployeeStatus.Active
        };

        await _unitOfWork.Employees.AddAsync(employee);
        await _unitOfWork.SaveChangesAsync();

        await _logService.LogCreateAsync("Employee", employee.Id);

        return ResponseHelper.Success(
            MapToResponse(employee, department.Name, designation.Title),
            "Employee created successfully.", "201");
    }

    public async Task<StandardResponse<EmployeeResponse>> UpdateAsync(Guid id, UpdateEmployeeRequest request)
    {
        var employee = await _unitOfWork.Employees.GetWithDetailsAsync(id)
            ?? throw new NotFoundException(nameof(Employee), id);

        var department = await _unitOfWork.Departments.GetByIdAsync(request.DepartmentId)
            ?? throw new NotFoundException(nameof(Department), request.DepartmentId);

        var designation = await _unitOfWork.Designations.GetByIdAsync(request.DesignationId)
            ?? throw new NotFoundException(nameof(Designation), request.DesignationId);

        employee.FirstName = request.FirstName;
        employee.LastName = request.LastName;
        employee.Phone = request.Phone;
        employee.BasicSalary = request.BasicSalary;
        employee.DepartmentId = request.DepartmentId;
        employee.DesignationId = request.DesignationId;
        employee.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Employees.Update(employee);
        await _unitOfWork.SaveChangesAsync();

        await _logService.LogUpdateAsync("Employee", employee.Id);

        return ResponseHelper.Success(
            MapToResponse(employee, department.Name, designation.Title),
            "Employee updated successfully.");
    }

    public async Task<StandardResponse<EmployeeResponse>> GetByIdAsync(Guid id)
    {
        var employee = await _unitOfWork.Employees.GetWithDetailsAsync(id)
            ?? throw new NotFoundException(nameof(Employee), id);

        return ResponseHelper.Success(
            MapToResponse(employee, employee.Department.Name, employee.Designation.Title));
    }

    public async Task<StandardResponse<List<EmployeeResponse>>> GetAllAsync()
    {
        var employees = await _unitOfWork.Employees.GetAllWithDetailsAsync();
        var response = employees
            .Select(e => MapToResponse(e, e.Department.Name, e.Designation.Title))
            .ToList();

        return ResponseHelper.Success(response, $"{response.Count} employees retrieved.");
    }

    public async Task<StandardResponse<bool>> DeactivateAsync(Guid id)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Employee), id);

        employee.Status = EmployeeStatus.Inactive;
        employee.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Employees.Update(employee);
        await _unitOfWork.SaveChangesAsync();

        await _logService.LogUpdateAsync("Employee", id, "Employee deactivated.");

        return ResponseHelper.Success(true, "Employee deactivated successfully.");
    }

    private static EmployeeResponse MapToResponse(
        Employee e, string departmentName, string designationTitle) => new()
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            Phone = e.Phone,
            DateOfBirth = e.DateOfBirth,
            JoiningDate = e.JoiningDate,
            BasicSalary = e.BasicSalary,
            Status = e.Status.ToString(),
            DepartmentName = departmentName,
            DesignationTitle = designationTitle,
            CreatedAt = e.CreatedAt
        };
}