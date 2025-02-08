using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TimesheetApp.Controllers;
using Moq;
using TimesheetApp.Interfaces;
using Timesheet.Models;
using TimesheetApp.Models;

namespace TimesheetApp.Tests;

public class HomeControllerTests
{
    private ILogger<HomeController> _logger;
    private Mock<ITimesheetRepository> _timesheetRepoMock;

    [SetUp]
    public void Setup()
    {
        _timesheetRepoMock = new Mock<ITimesheetRepository>();
        _logger = new Mock<ILogger<HomeController>>().Object;
    }

    [Test]
    public async Task Index_ReturnsAViewResult_WithAListOfTimesheets()
    {
        //Arrange
        _timesheetRepoMock.Setup(repo => repo.ListAsync())
            .ReturnsAsync(GetTestTimesheets());
        var controller = new HomeController(_logger, _timesheetRepoMock.Object);

        // Act
        var result = await controller.IndexAsync();

        // Assert
        Assert.That(result, Is.InstanceOf<ViewResult>());
        Assert.That(((ViewResult)result).ViewData.Model, Is.InstanceOf<IList<TimesheetModel>>());
        IList<TimesheetModel> results = (IList<TimesheetModel>)((ViewResult)result).ViewData.Model;
        Assert.That(results.Count, Is.EqualTo(2));
        Assert.That(results.First().TotalHoursWorked, Is.EqualTo(9));
    }

    private ICollection<TimesheetRow> GetTestTimesheets()
    {
        var timesheetRows = new List<TimesheetRow>
        {
            new TimesheetRow()
            {
                ProjectName = "Mars",
                Description = "New Feature",
                HoursWorked = 7,
                Timesheet = new Timesheet.Models.Timesheet()
                {
                    Username = "aliyaaziz",
                    Date = DateTime.Today,
                    TimesheetRows = [new TimesheetRow() {
                        ProjectName = "Mars",
                        Description = "New Feature",
                        HoursWorked = 7
                    },
                    new TimesheetRow() {
                        ProjectName = "Pluto",
                        Description = "Bug Fix",
                        HoursWorked = 2
                    }]
                }
            },
            new TimesheetRow()
            {
                ProjectName = "Pluto",
                Description = "Bug Fix",
                HoursWorked = 2,
                Timesheet = new Timesheet.Models.Timesheet()
                {
                    Username = "aliyaaziz",
                    Date = DateTime.Today,
                    TimesheetRows = [new TimesheetRow() {
                        ProjectName = "Mars",
                        Description = "New Feature",
                        HoursWorked = 7
                    },
                    new TimesheetRow() {
                        ProjectName = "Pluto",
                        Description = "Bug Fix",
                        HoursWorked = 2
                    }]
                }
            }
        };
       
        return timesheetRows;
    }
}
