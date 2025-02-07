namespace Timesheet.Models;

public class TimesheetRow
{
    public int Id { get; set; }
    public int TimesheetId { get; set; }
    public required string ProjectName { get; set; }
    public required string Description { get; set; }
    public int HoursWorked { get; set; }

    public Timesheet Timesheet { get; set; } = null!;
}
