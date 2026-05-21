using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Persistence.Configurations;

public class DesignationConfiguration : BaseEntityConfiguration<Designation>
{
    public override void Configure(EntityTypeBuilder<Designation> builder)
    {
        base.Configure(builder);

        builder.ToTable("Designations");

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        // Designation → Department (Many to One)
        builder.HasOne(x => x.Department)
            .WithMany(x => x.Designations)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}