using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TimesheetApp.Validators;
using FluentValidation.Attributes;

namespace TimesheetApp.Models;

[Validator(typeof(TimesheetCreateModelValidator))]
public class TimesheetCreateModel
{
    public required string Username { get; set; }
    public DateTime Date {get; set; }
    public required string Project { get; set; }
    public required string Description { get; set; }
    [DisplayName("Hours Worked")]
    public int HoursWorked { get; set; }
}
