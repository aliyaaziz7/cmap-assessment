using Microsoft.EntityFrameworkCore;

namespace Timesheet.Models;

public class TimesheetContext(DbContextOptions options) : DbContext(options)
{
    public required DbSet<Timesheet> Timesheets { get; set; }
    public required DbSet<TimesheetRow> TimesheetRows { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    
        modelBuilder.Entity<Timesheet>()
        .HasMany(e => e.TimesheetRows)
        .WithOne(e => e.Timesheet)
        .HasForeignKey(e => e.TimesheetId)
        .IsRequired();
    }
}
