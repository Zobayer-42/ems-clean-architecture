using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Application.DTO.Request.Employee;
using EmployeeManagement.Application.DTO.Response.Employee;
using EmployeeManagement.Shared.Response;

namespace EmployeeManagement.Application.Services.Interfaces;

public interface IEmployeeService
{
    Task<StandardResponse<EmployeeResponse>> CreateAsync(CreateEmployeeRequest request);
    Task<StandardResponse<EmployeeResponse>> UpdateAsync(Guid id, UpdateEmployeeRequest request);
    Task<StandardResponse<EmployeeResponse>> GetByIdAsync(Guid id);
    Task<StandardResponse<List<EmployeeResponse>>> GetAllAsync();
    Task<StandardResponse<bool>> DeactivateAsync(Guid id);
}