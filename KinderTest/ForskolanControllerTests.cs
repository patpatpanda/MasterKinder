using Xunit;
using MasterKinderAPI.Controllers;
using MasterKinder.Data;
using MasterKinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

public class SurveyControllerTests
{
    private MrDb GetInMemoryDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<MrDb>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        var context = new MrDb(options);

        // Ensure the database is deleted to start fresh each time
        context.Database.EnsureDeleted();

        // Seed the database with test data
        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "XYZ", Fragetext = "ABC" });
        context.SaveChanges();

        return context;
    }

    [Fact]
    public async Task GetSurveyResponses_ReturnsOkResult_WithValidParameters()
    {
        // Arrange
        var context = GetInMemoryDbContext("TestDb1");
        var controller = new SurveyController(context);

        // Act
        var result = await controller.GetSurveyResponses(2020, "XYZ", "ABC");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<ISurveyResponse>>(okResult.Value);
        Assert.Single(returnValue);  // Check that one item is returned
        Assert.Equal("XYZ", returnValue[0].Forskoleverksamhet);
        Assert.Equal("ABC", returnValue[0].Fragetext);
    }

    [Fact]
    public async Task GetSurveyResponses_ReturnsBadRequest_WhenParametersAreMissing()
    {
        // Arrange
        var context = GetInMemoryDbContext("TestDb2");
        var controller = new SurveyController(context);

        // Act
        var result = await controller.GetSurveyResponses(2020, "", "ABC");

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Förskoleverksamhet och Frågetext måste anges.", badRequestResult.Value);
    }
}
