using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Models;

namespace EmployeeManagement.Persistence.Repositories.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<Employee?> GetWithDetailsAsync(Guid id);
    Task<List<Employee>> GetAllWithDetailsAsync();
    Task<bool> IsEmailExistsAsync(string email, Guid? excludeId = null);
}