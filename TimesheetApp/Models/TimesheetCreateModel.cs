using System.ComponentModel;

namespace TimesheetApp.Models;

public class TimesheetCreateModel
{
    public required string Username { get; set; }
    public DateTime Date {get; set; }
    public required string Project { get; set; }
    public required string Description { get; set; }
    [DisplayName("Hours Worked")]
    public int HoursWorked { get; set; }
}
