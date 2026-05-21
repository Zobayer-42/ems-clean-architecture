using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Common;
using EmployeeManagement.Domain.Enums;

namespace EmployeeManagement.Domain.Models
{
    public class Employee : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public DateOnly JoiningDate { get; set; }
        public decimal BasicSalary { get; set; }
        public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;

        public Guid DepartmentId { get; set; }
        public Guid DesignationId { get; set; }

        public Department Department { get; set; } = null!;
        public Designation Designation { get; set; } = null!;
        public ICollection<SalaryDisbursement> SalaryDisbursements { get; set; } = new List<SalaryDisbursement>();
    }
}
