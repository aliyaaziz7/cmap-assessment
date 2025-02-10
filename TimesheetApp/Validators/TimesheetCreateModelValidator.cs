using FluentValidation;
using TimesheetApp.Models;

namespace TimesheetApp.Validators;

public class TimesheetCreateModelValidator : AbstractValidator<TimesheetCreateModel>
{
    public TimesheetCreateModelValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required");

        RuleFor(x => x.Project)
            .NotEmpty()
            .WithMessage("Project is required");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required");

        RuleFor(x => x.HoursWorked)
            .NotEmpty()
            .WithMessage("Hours worked is required");

        RuleFor(x => x.HoursWorked)
            .GreaterThan(0)
            .WithMessage("Hours worked must be more than 0");

        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Date is required");

        RuleFor(x => x.Date)
            .LessThan(DateTime.Today.AddDays(1))
            .WithMessage("Date cannot be in the future");
    }
}