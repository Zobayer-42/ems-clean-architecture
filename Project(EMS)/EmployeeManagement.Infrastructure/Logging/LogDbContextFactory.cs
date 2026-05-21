using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EmployeeManagement.Infrastructure.Logging;
public class LogDbContextFactory : IDesignTimeDbContextFactory<LogDbContext>
{
    public LogDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<LogDbContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=EmsLogs;Username=postgres;Password=1234")
            .Options;
        return new LogDbContext(options);
    }
}