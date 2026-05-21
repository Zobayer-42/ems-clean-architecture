using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Enums;
using EmployeeManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Persistence.Configurations;

public class SalaryDisbursementConfiguration : BaseEntityConfiguration<SalaryDisbursement>
{
    public override void Configure(EntityTypeBuilder<SalaryDisbursement> builder)
    {
        base.Configure(builder);

        builder.ToTable("SalaryDisbursements");

        builder.Property(x => x.BasicSalary)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Bonus)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Deduction)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.NetSalary)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasDefaultValue(PaymentStatus.Pending);

        builder.Property(x => x.Remarks)
            .HasMaxLength(500);

        builder.HasIndex(x => new { x.EmployeeId, x.Month, x.Year })
            .IsUnique();

        builder.HasOne(x => x.Employee)
            .WithMany(x => x.SalaryDisbursements)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}