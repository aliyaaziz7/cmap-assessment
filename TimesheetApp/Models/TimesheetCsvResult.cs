using Microsoft.AspNetCore.Mvc;

namespace TimesheetApp.Models;

public class TimesheetCsvResult : FileResult
{
    private readonly IEnumerable<TimesheetModel> _timesheetData;

    public TimesheetCsvResult(IEnumerable<TimesheetModel> timesheetData, string fileDownloadName) : base("text/csv")
    {
        _timesheetData = timesheetData;
        FileDownloadName = fileDownloadName;
    }

    public async override Task ExecuteResultAsync(ActionContext context)
    {
        var response = context.HttpContext.Response;
        context.HttpContext.Response.Headers.Append("Content-Disposition", new[] { "attachment; filename=" + FileDownloadName });

        using var streamWriter = new StreamWriter(response.Body);
        
        await streamWriter.WriteLineAsync(
          $"User Name, Date, Project, Description of Tasks, Hours Worked, Total Hours for the Day"
        );

        foreach (var p in _timesheetData)
        {
            await streamWriter.WriteLineAsync(
              $"{p.Username}, {p.Date}, {p.Project}, {p.Description}, {p.HoursWorked}, {p.TotalHoursWorked}"
            );
        
            await streamWriter.FlushAsync();
        }
        
        await streamWriter.FlushAsync();
    }
}