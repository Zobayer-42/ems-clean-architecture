using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Common;
using EmployeeManagement.Domain.Enums;

namespace EmployeeManagement.Domain.Models
{
    public class SalaryDisbursement : BaseEntity
    {
        public Guid EmployeeId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Bonus { get; set; }
        public decimal Deduction { get; set; }
        public decimal NetSalary { get; set; }
        public DateTime DisbursedAt { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public string? Remarks { get; set; }

        public Employee Employee { get; set; } = null!;
    }
}
