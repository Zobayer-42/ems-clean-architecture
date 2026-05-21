using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Persistence.Configurations;

public class PermissionConfiguration : BaseEntityConfiguration<Permission>
{
    public override void Configure(EntityTypeBuilder<Permission> builder)
    {
        base.Configure(builder);

        builder.ToTable("Permissions");

        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Module).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Action).IsRequired().HasMaxLength(50);

        builder.HasIndex(x => x.Name).IsUnique();

        var createdAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        builder.HasData(
            new Permission { Id = Guid.Parse("b1b2c3d4-0001-0000-0000-000000000001"), Name = "employee.create", Module = "Employee", Action = "Create", CreatedAt = createdAt },
            new Permission { Id = Guid.Parse("b1b2c3d4-0001-0000-0000-000000000002"), Name = "employee.read", Module = "Employee", Action = "Read", CreatedAt = createdAt },
            new Permission { Id = Guid.Parse("b1b2c3d4-0001-0000-0000-000000000003"), Name = "employee.update", Module = "Employee", Action = "Update", CreatedAt = createdAt },
            new Permission { Id = Guid.Parse("b1b2c3d4-0001-0000-0000-000000000004"), Name = "employee.delete", Module = "Employee", Action = "Delete", CreatedAt = createdAt },
            new Permission { Id = Guid.Parse("b1b2c3d4-0001-0000-0000-000000000005"), Name = "department.create", Module = "Department", Action = "Create", CreatedAt = createdAt },
            new Permission { Id = Guid.Parse("b1b2c3d4-0001-0000-0000-000000000006"), Name = "department.read", Module = "Department", Action = "Read", CreatedAt = createdAt },
            new Permission { Id = Guid.Parse("b1b2c3d4-0001-0000-0000-000000000007"), Name = "department.update", Module = "Department", Action = "Update", CreatedAt = createdAt },
            new Permission { Id = Guid.Parse("b1b2c3d4-0001-0000-0000-000000000008"), Name = "department.delete", Module = "Department", Action = "Delete", CreatedAt = createdAt },
            new Permission { Id = Guid.Parse("b1b2c3d4-0001-0000-0000-000000000009"), Name = "salary.disburse", Module = "Salary", Action = "Disburse", CreatedAt = createdAt },
            new Permission { Id = Guid.Parse("b1b2c3d4-0001-0000-0000-000000000010"), Name = "salary.read", Module = "Salary", Action = "Read", CreatedAt = createdAt },
            new Permission { Id = Guid.Parse("b1b2c3d4-0001-0000-0000-000000000011"), Name = "report.read", Module = "Report", Action = "Read", CreatedAt = createdAt }
        );
    }
}