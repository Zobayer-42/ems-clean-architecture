using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmployeeManagement.Application.Contracts;
using EmployeeManagement.Infrastructure.Logging;
using EmployeeManagement.Infrastructure.Security;
using EmployeeManagement.Shared.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;


namespace EmployeeManagement.Configuration.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // PostgreSQL — Log Database
        services.AddDbContext<LogDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("PostgresConnection"),
                npgsql => npgsql.MigrationsAssembly(
                    typeof(LogDbContext).Assembly.FullName)));

        services.AddHttpContextAccessor();

        // Security Services
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        // Log Service
        services.AddScoped<ILogService, LogService>();

        // JwtSettings config bind
        services.Configure<JwtSettings>(
            configuration.GetSection("JwtSettings"));

        // JWT Authentication
        var jwtSettings = configuration
            .GetSection("JwtSettings")
            .Get<JwtSettings>()!;

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                ClockSkew = TimeSpan.Zero
            };
        });

        // Permission-based Authorization
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

        services.AddAuthorizationBuilder()
            .AddPolicy("employee.create", p => p.AddRequirements(new PermissionRequirement("employee.create")))
            .AddPolicy("employee.read", p => p.AddRequirements(new PermissionRequirement("employee.read")))
            .AddPolicy("employee.update", p => p.AddRequirements(new PermissionRequirement("employee.update")))
            .AddPolicy("employee.delete", p => p.AddRequirements(new PermissionRequirement("employee.delete")))
            .AddPolicy("department.create", p => p.AddRequirements(new PermissionRequirement("department.create")))
            .AddPolicy("department.read", p => p.AddRequirements(new PermissionRequirement("department.read")))
            .AddPolicy("department.update", p => p.AddRequirements(new PermissionRequirement("department.update")))
            .AddPolicy("department.delete", p => p.AddRequirements(new PermissionRequirement("department.delete")))
            .AddPolicy("salary.disburse", p => p.AddRequirements(new PermissionRequirement("salary.disburse")))
            .AddPolicy("salary.read", p => p.AddRequirements(new PermissionRequirement("salary.read")))
            .AddPolicy("report.read", p => p.AddRequirements(new PermissionRequirement("report.read")));

        return services;
    }
}