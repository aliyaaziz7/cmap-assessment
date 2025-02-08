using Timesheet.Models;

namespace TimesheetApp.Interfaces;

public interface ITimesheetRepository
{
    Task<ICollection<TimesheetRow>> ListAsync();
    Task AddAsync(Timesheet.Models.Timesheet timesheet);
}