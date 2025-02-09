using Timesheet.Models;

public class DbInitialiser
{
    public static void InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TimesheetContext>()
        ?? throw new InvalidOperationException("Failed to retrieve timesheet context");

        SeedData(context);
    }

    private static void SeedData(TimesheetContext context)
    {
        if(context.Timesheets.Any()) {
            return;
        }

        var timesheet = new Timesheet.Models.Timesheet() {
            Username = "aliyaaziz",
            Date = DateTime.Today
        };

        var timesheet2 = new Timesheet.Models.Timesheet() {
            Username = "kate123",
            Date = DateTime.Today
        };

        context.Timesheets.Add(timesheet);
        context.Timesheets.Add(timesheet2);

        timesheet.TimesheetRows.Add(new TimesheetRow() {
            ProjectName = "Icarus",
            Description = "Debugging",
            HoursWorked = 4
        });

        timesheet.TimesheetRows.Add(new TimesheetRow() {
            ProjectName = "Jupiter",
            Description = "New Feature",
            HoursWorked = 3
        });

        timesheet.TimesheetRows.Add(new TimesheetRow() {
            ProjectName = "Mars",
            Description = "New Feature",
            HoursWorked = 2
        });

        timesheet2.TimesheetRows.Add(new TimesheetRow() {
            ProjectName = "Mars",
            Description = "New Feature",
            HoursWorked = 7
        });

        context.SaveChanges();
    }
}
