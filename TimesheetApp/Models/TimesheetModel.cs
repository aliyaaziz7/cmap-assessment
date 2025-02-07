namespace TimesheetApp.Models;

public class TimesheetModel
{
    public required string Username { get; set; }
    public DateTime Date { get; set; }
    public required string Project { get; set; }
    public required string Description { get; set; }
    public int HoursWorked { get; set; }
    public int TotalHoursWorked { get; set; }
}
