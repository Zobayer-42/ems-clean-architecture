using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Domain.Models
{
    public class Permission : BaseEntity
    {
        public string Name { get; set; } = string.Empty;   
        public string Module { get; set; } = string.Empty; 
        public string Action { get; set; } = string.Empty; 

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
