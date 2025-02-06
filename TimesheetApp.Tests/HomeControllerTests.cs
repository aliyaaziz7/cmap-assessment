using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TimesheetApp.Controllers;
using Moq;

namespace TimesheetApp.Tests;

public class HomeControllerTests
{
    private ILogger<HomeController> _logger;
    
    [SetUp]
    public void Setup()
    {
        var mock = new Mock<ILogger<HomeController>>();
        _logger = mock.Object;
    }

    [Test]
    public void Test1()
    {
        var controller = new HomeController(_logger);
        var result = controller.Index() as ViewResult;
        Assert.That(result?.ViewName, Is.EqualTo("Index"));
    }
}
