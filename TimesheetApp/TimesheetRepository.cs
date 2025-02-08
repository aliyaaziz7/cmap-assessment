using Microsoft.EntityFrameworkCore;
using Timesheet.Models;
using TimesheetApp.Interfaces;

namespace TimesheetApp;

public class TimesheetRepository : ITimesheetRepository
{
    private readonly TimesheetContext _context;

    public TimesheetRepository(TimesheetContext context)
    {
        _context = context;
    }

    public async Task<ICollection<TimesheetRow>> ListAsync()
    {
        return await _context.TimesheetRows
                .Include(x => x.Timesheet)
                .ThenInclude(x => x.TimesheetRows).ToListAsync();
    }

    public async Task AddAsync(Timesheet.Models.Timesheet timesheet)
    {
        var existingTimesheet = await _context.Timesheets.Where(x => x.Username.Equals(timesheet.Username) && x.Date.Date.Equals(timesheet.Date.Date)).AnyAsync();
        
        if(!existingTimesheet) {
            var timesheet1 = new Timesheet.Models.Timesheet() {
                Username = timesheet.Username,
                Date = timesheet.Date
            };
            _context.Timesheets.Add(timesheet1);
            timesheet1.TimesheetRows.Add(new TimesheetRow() {
                ProjectName = timesheet.TimesheetRows.First().ProjectName,
                Description = timesheet.TimesheetRows.First().Description,
                HoursWorked = timesheet.TimesheetRows.First().HoursWorked
            });
        } 
        else {
             var timesheet1 = await _context.Timesheets.Where(x => x.Username.Equals(timesheet.Username) && x.Date.Date.Equals(timesheet.Date.Date)).FirstAsync();
             timesheet1.TimesheetRows.Add(new TimesheetRow() {
                ProjectName = timesheet.TimesheetRows.First().ProjectName,
                Description = timesheet.TimesheetRows.First().Description,
                HoursWorked = timesheet.TimesheetRows.First().HoursWorked
            });
        }
        
        await _context.SaveChangesAsync();
    }
}