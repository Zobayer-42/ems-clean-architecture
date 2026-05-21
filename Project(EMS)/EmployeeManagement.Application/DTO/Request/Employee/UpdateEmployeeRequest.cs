using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.DTO.Request.Employee
{
    public class UpdateEmployeeRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public decimal BasicSalary { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid DesignationId { get; set; }
    }
}
