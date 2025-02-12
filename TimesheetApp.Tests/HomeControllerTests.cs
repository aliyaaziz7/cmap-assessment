using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TimesheetApp.Controllers;
using TimesheetApp.Interfaces;
using Timesheet.Models;
using TimesheetApp.Models;
using Moq;
using Microsoft.AspNetCore.Http;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace TimesheetApp.Tests;

public class HomeControllerTests
{
    private IValidator<TimesheetCreateModel> _validator;
    private ILogger<HomeController> _logger;
    private Mock<ITimesheetRepository> _timesheetRepoMock;
    private DefaultHttpContext _httpContext;
    private ActionContext _fakeActionContext;

    [SetUp]
    public void Setup()
    {
        _timesheetRepoMock = new Mock<ITimesheetRepository>();
        _logger = new Mock<ILogger<HomeController>>().Object;
        var validatorMock = new Mock<IValidator<TimesheetCreateModel>>();
        validatorMock
            .Setup(repo => repo.ValidateAsync(It.IsAny<TimesheetCreateModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(){ Errors = [], RuleSetsExecuted = []});
        _validator = validatorMock.Object;
        _httpContext = new DefaultHttpContext();
        _fakeActionContext = new ActionContext()
        {
            HttpContext = _httpContext
        };
    }

    [Test]
    public async Task Index_ReturnsAViewResult_WithAListOfTimesheets()
    {
        //Arrange
        _timesheetRepoMock.Setup(repo => repo.ListAsync())
            .ReturnsAsync(GetTestTimesheets());
        var controller = new HomeController(_logger, _timesheetRepoMock.Object, _validator);
        
        // Act
        var result = await controller.IndexAsync();

        // Assert
        Assert.That(result, Is.InstanceOf<ViewResult>());
        Assert.That(((ViewResult)result).ViewData.Model, Is.InstanceOf<IList<TimesheetModel>>());
        IList<TimesheetModel> results = (IList<TimesheetModel>)((ViewResult)result).ViewData.Model;
        Assert.That(results.Count, Is.EqualTo(2));
        Assert.That(results.First().TotalHoursWorked, Is.EqualTo(9));
    }

    [Test]
    public async Task CreatePost_ReturnsAViewResult_WhenModelStateIsInvalid()
    {
        //Arrange
        _timesheetRepoMock.Setup(repo => repo.ListAsync())
            .ReturnsAsync(GetTestTimesheets());
        var validatorMock = new Mock<IValidator<TimesheetCreateModel>>();
        validatorMock
            .Setup(repo => repo.ValidateAsync(It.IsAny<TimesheetCreateModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(){ 
                Errors = [new ValidationFailure(){ ErrorMessage = "Validation Error", PropertyName = "Username" }], 
                RuleSetsExecuted = []});  
        _validator = validatorMock.Object;
        var controller = new HomeController(_logger, _timesheetRepoMock.Object, _validator);
        var tempData = new TempDataDictionary(_httpContext, Mock.Of<ITempDataProvider>());
        controller.TempData = tempData;
        
        // Act
        var result = await controller.CreateTimesheet(GetTestTimesheetModel());

        // Assert
        Assert.That(result, Is.InstanceOf<ViewResult>());
        Assert.That(((ViewResult)result).ViewData.Model, Is.InstanceOf<TimesheetCreateModel>());
    }

    [Test]
    public async Task CreatePost_ReturnsARedirect_WhenModelStateIsValid()
    {
        //Arrange
        _timesheetRepoMock.Setup(repo => repo.ListAsync())
            .ReturnsAsync(GetTestTimesheets());
        var controller = new HomeController(_logger, _timesheetRepoMock.Object, _validator);
        var tempData = new TempDataDictionary(_httpContext, Mock.Of<ITempDataProvider>());
        controller.TempData = tempData;
        
        // Act
        var result = await controller.CreateTimesheet(GetTestTimesheetModel());

        // Assert
        Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
        Assert.That(((RedirectToActionResult)result).ActionName, Is.EqualTo("Index"));
        Assert.That(((RedirectToActionResult)result).ControllerName, Is.EqualTo("Home"));
    }

    [Test]
    public async Task ExportCsv_ReturnsACsv()
    {
        //Arrange
        _timesheetRepoMock.Setup(repo => repo.ListAsync())
            .ReturnsAsync(GetTestTimesheets());
        var controller = new HomeController(_logger, _timesheetRepoMock.Object, _validator);
        
        // Act
        var result = await controller.ExportCsvAsync();

        // Assert
        Assert.That(result, Is.InstanceOf<TimesheetCsvResult>());
        Assert.That(result.FileDownloadName, Is.EqualTo("timesheet.csv"));
        Assert.That(result.ContentType, Is.EqualTo("text/csv"));
    }

    [Test]
    public async Task TimesheetCsvResult_ReturnsExpectedCsvFormat()
    {
        //Arrange
        var data = new List<TimesheetModel>()
            {
                new() { Username = "aliyaaziz", Date = DateTime.Parse("09/02/2025"), Project = "Icarus", Description = "Debugging", HoursWorked = 4, TotalHoursWorked = 9 },
                new() { Username = "aliyaaziz", Date = DateTime.Parse("09/02/2025"), Project = "Jupiter", Description = "New Feature", HoursWorked = 3, TotalHoursWorked = 9 },
                new() { Username = "aliyaaziz", Date = DateTime.Parse("09/02/2025"), Project = "Mars", Description = "New Feature", HoursWorked = 2, TotalHoursWorked = 9 },
                new() { Username = "kate123", Date = DateTime.Parse("09/02/2025"), Project = "Mars", Description = "New Feature", HoursWorked = 7, TotalHoursWorked = 7 },
            }.AsQueryable();
        var _expectedResponseText = File.ReadAllText(TestContext.CurrentContext.TestDirectory + @"/TestData/expectedCsv.txt");
        var _timesheetCsvResult = new TimesheetCsvResult(data, "timesheet.csv");
        var memoryStream = new MemoryStream();
        _httpContext.Response.Body = memoryStream;

        //Act
        await _timesheetCsvResult.ExecuteResultAsync(_fakeActionContext);
        var streamText = System.Text.Encoding.Default.GetString(memoryStream.ToArray());

        // Assert
        Assert.That(streamText, Is.EqualTo(_expectedResponseText));
    }

    private static ICollection<TimesheetRow> GetTestTimesheets()
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
            new()
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

    private static TimesheetCreateModel GetTestTimesheetModel()
    {
        return new TimesheetCreateModel(){
            Username = "a",
            Date = DateTime.Today,
            Project = "a",
            Description = "a",
            HoursWorked = 1
        };
    }
}
