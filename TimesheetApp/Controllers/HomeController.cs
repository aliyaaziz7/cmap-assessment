using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Timesheet.Models;
using TimesheetApp.Models;

namespace TimesheetApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly TimesheetContext _context;

    public HomeController(ILogger<HomeController> logger, TimesheetContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var viewModel = _context.TimesheetRows
                .Include(x => x.Timesheet)
                .ThenInclude(x => x.TimesheetRows)
                .Select(x => new TimesheetModel() {
            Username = x.Timesheet.Username,
            Project = x.ProjectName,
            Description = x.Description,
            HoursWorked = x.HoursWorked,
            TotalHoursWorked = x.Timesheet.TotalHours,
            Date = x.Timesheet.Date
        }).ToList();
            
        return View("List", viewModel);
    }

    public IActionResult Create()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
