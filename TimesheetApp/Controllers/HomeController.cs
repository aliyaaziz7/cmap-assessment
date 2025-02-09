using System.Diagnostics;
using Humanizer.Bytes;
using Microsoft.AspNetCore.Mvc;
using Timesheet.Models;
using TimesheetApp.Interfaces;
using TimesheetApp.Models;

namespace TimesheetApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ITimesheetRepository _timesheetRepository;

    public HomeController(ILogger<HomeController> logger, ITimesheetRepository timesheetRepository)
    {
        _logger = logger;
        _timesheetRepository = timesheetRepository;
    }

    public async Task<IActionResult> IndexAsync()
    {
        var viewModel = (await _timesheetRepository.ListAsync())
                .Select(x => new TimesheetModel()
                {
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> CreateTimesheet(TimesheetCreateModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Create", model);
        }

        await AddNewTimesheet(model);
        
        return RedirectToAction("Index", "Home");
    }

    public async Task AddNewTimesheet(TimesheetCreateModel model) {
        await _timesheetRepository.AddAsync(new Timesheet.Models.Timesheet() {
            Username = model.Username,
            Date = model.Date,
            TimesheetRows = [new TimesheetRow() {
                ProjectName = model.Project,
                Description = model.Description,
                HoursWorked = model.HoursWorked
            }]
        });
    }

    public FileContentResult ExportCsv()
    {
        string csv = "";
        return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "timesheet.csv");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
