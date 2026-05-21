using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Application.DTO.Request.Department;
using EmployeeManagement.Application.DTO.Response.Department;
using EmployeeManagement.Shared.Response;

namespace EmployeeManagement.Application.Services.Interfaces;

public interface IDepartmentService
{
    Task<StandardResponse<DepartmentResponse>> CreateAsync(CreateDepartmentRequest request);
    Task<StandardResponse<DepartmentResponse>> UpdateAsync(Guid id, UpdateDepartmentRequest request);
    Task<StandardResponse<DepartmentResponse>> GetByIdAsync(Guid id);
    Task<StandardResponse<List<DepartmentResponse>>> GetAllAsync();
    Task<StandardResponse<bool>> DeleteAsync(Guid id);
}