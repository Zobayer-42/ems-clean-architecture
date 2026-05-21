using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
namespace EmployeeManagement.Infrastructure.Logging;
public class LogDbContext : DbContext
{
    public LogDbContext(DbContextOptions<LogDbContext> options) : base(options) { }

    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.ToTable("activity_logs"); 

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id)
                .UseIdentityAlwaysColumn();  

            entity.Property(x => x.Action)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.Module)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.UserId)
                .HasMaxLength(100);

            entity.Property(x => x.Username)
                .HasMaxLength(100);

            entity.Property(x => x.IpAddress)
                .HasMaxLength(50);

            entity.Property(x => x.Details)
                .HasColumnType("text");

            entity.Property(x => x.ErrorMessage)
                .HasMaxLength(1000);

            entity.Property(x => x.CreatedAt)
                .HasDefaultValueSql("NOW()");

            entity.HasIndex(x => x.CreatedAt);
            entity.HasIndex(x => x.UserId);
            entity.HasIndex(x => x.Action);
        });
    }
}
