using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeManagement.Application.DTO.Request.Salary;
using FluentValidation;

namespace EmployeeManagement.Application.Validators;

public class SalaryDisbursementValidator : AbstractValidator<SalaryDisbursementRequest>
{
    public SalaryDisbursementValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee is required.");

        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12).WithMessage("Month must be between 1 and 12.");

        RuleFor(x => x.Year)
            .InclusiveBetween(2000, DateTime.UtcNow.Year)
            .WithMessage($"Year must be between 2000 and {DateTime.UtcNow.Year}.");

        RuleFor(x => x.Bonus)
            .GreaterThanOrEqualTo(0).WithMessage("Bonus cannot be negative.");

        RuleFor(x => x.Deduction)
            .GreaterThanOrEqualTo(0).WithMessage("Deduction cannot be negative.");
    }
}