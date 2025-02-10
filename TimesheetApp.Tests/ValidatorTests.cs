using TimesheetApp.Models;
using TimesheetApp.Validators;

namespace TimesheetApp.Tests;

public class ValidaorTests
{

    [SetUp]
    public void Setup()
    {
    }


    [Test]
    public void TimesheetCreateModelValidator_ShouldNotErrorWhenModelIsValid()
    {
        // Arrange
        var validator = new TimesheetCreateModelValidator();
        var testRequest = new TimesheetCreateModel(){
            Username = "123",
            Project = "qwerty",
            Description = "test",
            Date = DateTime.Today.AddDays(-1),
            HoursWorked = 1
        };

        // Act
        var result = validator.Validate(testRequest);
        
        // Assert
        Assert.That(result.Errors.Count == 0);
    }

    [Test]
    public void TimesheetCreateModelValidator_ShouldErrorWhenDateIsInFuture()
    {
        // Arrange
        var validator = new TimesheetCreateModelValidator();
        var testRequest = new TimesheetCreateModel(){
            Username = "123",
            Project = "qwerty",
            Description = "test",
            Date = DateTime.Today.AddDays(2),
            HoursWorked = 1
        };

        // Act
        var result = validator.Validate(testRequest);
        
        // Assert
        Assert.That(result.Errors.Any(o => o.PropertyName == "Date"));
    }

    [Test]
    public void TimesheetCreateModelValidator_ShouldErrorWhenHoursIsLessThanZero()
    {
        // Arrange
        var validator = new TimesheetCreateModelValidator();
        var testRequest = new TimesheetCreateModel(){
            Username = "123",
            Project = "qwerty",
            Description = "test",
            Date = DateTime.Today.AddDays(-2),
            HoursWorked = -1
        };

        // Act
        var result = validator.Validate(testRequest);
        
        // Assert
        Assert.That(result.Errors.Any(o => o.PropertyName == "HoursWorked"));
    }
}
