using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Application.DTO.Request.Designation;
using EmployeeManagement.Application.DTO.Response.Designation;
using EmployeeManagement.Shared.Response;

namespace EmployeeManagement.Application.Services.Interfaces;

public interface IDesignationService
{
    Task<StandardResponse<DesignationResponse>> CreateAsync(CreateDesignationRequest request);
    Task<StandardResponse<DesignationResponse>> UpdateAsync(Guid id, UpdateDesignationRequest request);
    Task<StandardResponse<DesignationResponse>> GetByIdAsync(Guid id);
    Task<StandardResponse<List<DesignationResponse>>> GetAllAsync();
    Task<StandardResponse<bool>> DeleteAsync(Guid id);
}