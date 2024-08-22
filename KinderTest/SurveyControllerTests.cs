
using MasterKinderAPI.Controllers;
using MasterKinder.Data;
using MasterKinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


public class SurveyControllerTests
{
    private MrDb GetInMemoryDbContext(string dbName, bool seedData = false)
    {
        var options = new DbContextOptionsBuilder<MrDb>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        var context = new MrDb(options);

        // Ensure the database is deleted to start fresh each time
        context.Database.EnsureDeleted();

        // Seed the database with test data if specified
        if (seedData)
        {
            context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "XYZ", Fragetext = "ABC" });
            context.SaveChanges();
        }

        return context;
    }



    [Fact]
    public async Task GetSurveyResponses_ReturnsOkResult_WithValidParameters()
    {
        // Arrange
        var context = GetInMemoryDbContext("TestDb1", seedData: true); // Ensure data is seeded
        var controller = new SurveyController(context);

        // Act
        var result = await controller.GetSurveyResponses(2020, "XYZ", "ABC");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<ISurveyResponse>>(okResult.Value);

        // Logga antalet poster som returneras för felsökning
        Console.WriteLine($"Antal poster returnerade: {returnValue.Count}");

        // Kontrollera att en post returneras
        Assert.Single(returnValue);  // Förväntar sig en enda post
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



    [Fact]
    public async Task GetNöjdResponses_ReturnsOkResult_WithValidParameters()
    {
        // Arrange
        var context = GetInMemoryDbContext("TestDb3");
        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "XYZ", Fragetext = "ABC", SvarsalternativText = "3", Utfall = "10" });
        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "XYZ", Fragetext = "ABC", SvarsalternativText = "1", Utfall = "5" });
        await context.SaveChangesAsync();

        var controller = new SurveyController(context);

        // Act
        var result = await controller.GetNöjdResponses(2020, "XYZ", "ABC");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        // Casta det returnerade värdet till en känd anonym typ (det här fungerar bara om du vet exakt hur resultatet ser ut)
        var expectedType = new { AntalNöjdaSvar = 0, TotaltAntalSvar = 0 };
        var returnValue = okResult.Value.GetType().GetProperties()
            .ToDictionary(p => p.Name, p => p.GetValue(okResult.Value));

        Assert.NotNull(returnValue);
        Assert.Equal(10, returnValue["AntalNöjdaSvar"]);
        Assert.Equal(15, returnValue["TotaltAntalSvar"]);
    }

    


    [Fact]
    public async Task GetNöjdResponses_ReturnsBadRequest_WhenForskoleverksamhetIsMissing()
    {
        // Arrange
        var context = GetInMemoryDbContext("TestDb4");
        var controller = new SurveyController(context);

        // Act
        var result = await controller.GetNöjdResponses(2020, null, "ABC");

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Förskoleverksamhet måste anges.", badRequestResult.Value);
    }

    [Fact]
    public async Task GetNöjdResponses_ReturnsBadRequest_WhenFragetextAndFrageNrAreMissing()
    {
        // Arrange
        var context = GetInMemoryDbContext("TestDb5");
        var controller = new SurveyController(context);

        // Act
        var result = await controller.GetNöjdResponses(2020, "XYZ", null, null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Antingen Frågetext eller FrageNr måste anges.", badRequestResult.Value);
    }

    [Fact]
    public async Task GetNöjdResponses_ReturnsBadRequest_WhenYearIsInvalid()
    {
        // Arrange
        var context = GetInMemoryDbContext("TestDb6");
        var controller = new SurveyController(context);

        // Act
        var result = await controller.GetNöjdResponses(2019, "XYZ", "ABC");

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Ogiltigt år.", badRequestResult.Value);
    }

    [Fact]
    public async Task GetSvarsalternativResponses_ReturnsOkResult_WithValidParameters()
    {
        // Arrange
        var context = GetInMemoryDbContext("TestDb7");
        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "XYZ", Fragetext = "ABC", SvarsalternativText = "Alternativ1", Utfall = "5" });
        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "XYZ", Fragetext = "ABC", SvarsalternativText = "Alternativ2", Utfall = "10" });
        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "XYZ", Fragetext = "ABC", SvarsalternativText = "Alternativ1", Utfall = "15" });
        await context.SaveChangesAsync();

        var controller = new SurveyController(context);

        // Act
        var result = await controller.GetSvarsalternativResponses(2020, "XYZ", "ABC","25");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        // Kontrollera att returnerat värde är en lista
        var returnValue = okResult.Value as IEnumerable<object>;
        Assert.NotNull(returnValue);

        // Förväntade svar
        var expectedResults = new Dictionary<string, int>
    {
        { "Alternativ1", 20 }, // 5 + 15
        { "Alternativ2", 10 }
    };

        foreach (var item in returnValue)
        {
            var svarsalternativTextProperty = item.GetType().GetProperty("SvarsalternativText");
            var utfallProperty = item.GetType().GetProperty("Utfall");

            Assert.NotNull(svarsalternativTextProperty);
            Assert.NotNull(utfallProperty);

            var svarsalternativText = svarsalternativTextProperty.GetValue(item)?.ToString();
            var utfall = Convert.ToInt32(utfallProperty.GetValue(item));

            // Logga värdet för att felsöka
            Console.WriteLine($"SvarsalternativText: {svarsalternativText}, Utfall: {utfall}");

            // Kontrollera om svarsalternativText är null
            Assert.False(string.IsNullOrEmpty(svarsalternativText), "SvarsalternativText should not be null or empty");

            // Förhindra null-nycklar
            if (!string.IsNullOrEmpty(svarsalternativText))
            {
                Assert.True(expectedResults.ContainsKey(svarsalternativText), $"Unexpected SvarsalternativText: {svarsalternativText}");
                Assert.Equal(expectedResults[svarsalternativText], utfall);
            }
        }


    }
    [Fact]
    public async Task GetForskoleverksamheter_ReturnsOkResult_WithValidYear()
    {
        // Arrange
        var context = GetInMemoryDbContext("TestDb1");
        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "Förskola A" });
        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "Förskola B" });
        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "Förskola A" }); // Duplicate, should be ignored by Distinct()
        await context.SaveChangesAsync();

        var controller = new SurveyController(context);

        // Act
        var result = await controller.GetForskoleverksamheter(2020);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<string>>(okResult.Value);

        // Kontrollera att rätt antal unika förskoleverksamheter returneras
        Assert.Equal(2, returnValue.Count);
        Assert.Contains("Förskola A", returnValue);
        Assert.Contains("Förskola B", returnValue);
    }

    [Fact]
    public async Task GetForskoleverksamheter_ReturnsBadRequest_WithInvalidYear()
    {
        // Arrange
        var context = GetInMemoryDbContext("TestDb2");
        var controller = new SurveyController(context);

        // Act
        var result = await controller.GetForskoleverksamheter(2019); // Invalid year

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Ogiltigt år.", badRequestResult.Value);
    }

    [Fact]
    public async Task GetForskoleverksamheter_ReturnsEmptyList_WhenNoDataExists()
    {
        // Arrange
        var context = GetInMemoryDbContext("TestDb3");
        var controller = new SurveyController(context);

        // Act
        var result = await controller.GetForskoleverksamheter(2020);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<string>>(okResult.Value);

        // Kontrollera att listan är tom
        Assert.Empty(returnValue);
    }

}
