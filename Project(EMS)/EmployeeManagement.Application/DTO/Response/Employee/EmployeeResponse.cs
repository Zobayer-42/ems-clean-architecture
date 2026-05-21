using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.DTO.Response.Employee
{
    public class EmployeeResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public DateOnly JoiningDate { get; set; }
        public decimal BasicSalary { get; set; }
        public string Status { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string DesignationTitle { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
