namespace Timesheet.Models;

public class Timesheet
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public DateTime Date { get; set; }
    public int TotalHours => TimesheetRows != null ? TimesheetRows.Sum(x => x.HoursWorked) : 0;

    public ICollection<TimesheetRow> TimesheetRows = [];
}
