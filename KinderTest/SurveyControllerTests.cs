
//using MasterKinderAPI.Controllers;
//using MasterKinder.Data;
//using MasterKinder.Models;
//using Microsoft.AspNetCore.Mvc;
//using System.Text.Json;
//using Microsoft.EntityFrameworkCore;
//using static MasterKinderAPI.Controllers.SurveyController;


//public class SurveyControllerTests
//{
//    private MrDb GetInMemoryDbContext(string dbName, bool seedData = false)
//    {
//        var options = new DbContextOptionsBuilder<MrDb>()
//            .UseInMemoryDatabase(databaseName: dbName)
//            .Options;

//        var context = new MrDb(options);

//        // Ensure the database is deleted to start fresh each time
//        context.Database.EnsureDeleted();

//        // Seed the database with test data if specified
//        if (seedData)
//        {
//            context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "XYZ", Fragetext = "ABC" });
//            context.SaveChanges();
//        }

//        return context;
//    }



//    [Fact]
//    public async Task GetSurveyResponses_ReturnsOkResult_WithValidParameters()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDb1", seedData: true); // Ensure data is seeded
//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetSurveyResponses(2020, "XYZ", "ABC");

//        // Assert
//        var okResult = Assert.IsType<OkObjectResult>(result);
//        var returnValue = Assert.IsType<List<ISurveyResponse>>(okResult.Value);

//        // Logga antalet poster som returneras för felsökning
//        Console.WriteLine($"Antal poster returnerade: {returnValue.Count}");

//        // Kontrollera att en post returneras
//        Assert.Single(returnValue);  // Förväntar sig en enda post
//        Assert.Equal("XYZ", returnValue[0].Forskoleverksamhet);
//        Assert.Equal("ABC", returnValue[0].Fragetext);
//    }


//    [Fact]
//    public async Task GetSurveyResponses_ReturnsBadRequest_WhenParametersAreMissing()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDb2");
//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetSurveyResponses(2020, "", "ABC");

//        // Assert
//        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//        Assert.Equal("Förskoleverksamhet och Frågetext måste anges.", badRequestResult.Value);
//    }



//    [Fact]
//    public async Task GetNöjdResponses_ReturnsOkResult_WithValidParameters()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDb3");
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "XYZ", Fragetext = "ABC", SvarsalternativText = "3", Utfall = "10" });
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "XYZ", Fragetext = "ABC", SvarsalternativText = "1", Utfall = "5" });
//        await context.SaveChangesAsync();

//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetNöjdResponses(2020, "XYZ", "ABC");

//        // Assert
//        var okResult = Assert.IsType<OkObjectResult>(result);

//        // Casta det returnerade värdet till en känd anonym typ (det här fungerar bara om du vet exakt hur resultatet ser ut)
//        var expectedType = new { AntalNöjdaSvar = 0, TotaltAntalSvar = 0 };
//        var returnValue = okResult.Value.GetType().GetProperties()
//            .ToDictionary(p => p.Name, p => p.GetValue(okResult.Value));

//        Assert.NotNull(returnValue);
//        Assert.Equal(10, returnValue["AntalNöjdaSvar"]);
//        Assert.Equal(15, returnValue["TotaltAntalSvar"]);
//    }

    


//    [Fact]
//    public async Task GetNöjdResponses_ReturnsBadRequest_WhenForskoleverksamhetIsMissing()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDb4");
//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetNöjdResponses(2020, null, "ABC");

//        // Assert
//        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//        Assert.Equal("Förskoleverksamhet måste anges.", badRequestResult.Value);
//    }

//    [Fact]
//    public async Task GetNöjdResponses_ReturnsBadRequest_WhenFragetextAndFrageNrAreMissing()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDb5");
//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetNöjdResponses(2020, "XYZ", null, null);

//        // Assert
//        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//        Assert.Equal("Antingen Frågetext eller FrageNr måste anges.", badRequestResult.Value);
//    }

//    [Fact]
//    public async Task GetNöjdResponses_ReturnsBadRequest_WhenYearIsInvalid()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDb6");
//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetNöjdResponses(2019, "XYZ", "ABC");

//        // Assert
//        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//        Assert.Equal("Ogiltigt år.", badRequestResult.Value);
//    }

//    [Fact]
//    public async Task GetSvarsalternativResponses_ReturnsOkResult_WithValidParameters()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDb7");
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "XYZ", Fragetext = "ABC", SvarsalternativText = "Alternativ1", Utfall = "5" });
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "XYZ", Fragetext = "ABC", SvarsalternativText = "Alternativ2", Utfall = "10" });
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "XYZ", Fragetext = "ABC", SvarsalternativText = "Alternativ1", Utfall = "15" });
//        await context.SaveChangesAsync();

//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetSvarsalternativResponses(2020, "XYZ", "ABC","25");

//        // Assert
//        var okResult = Assert.IsType<OkObjectResult>(result);

//        // Kontrollera att returnerat värde är en lista
//        var returnValue = okResult.Value as IEnumerable<object>;
//        Assert.NotNull(returnValue);

//        // Förväntade svar
//        var expectedResults = new Dictionary<string, int>
//    {
//        { "Alternativ1", 20 }, // 5 + 15
//        { "Alternativ2", 10 }
//    };

//        foreach (var item in returnValue)
//        {
//            var svarsalternativTextProperty = item.GetType().GetProperty("SvarsalternativText");
//            var utfallProperty = item.GetType().GetProperty("Utfall");

//            Assert.NotNull(svarsalternativTextProperty);
//            Assert.NotNull(utfallProperty);

//            var svarsalternativText = svarsalternativTextProperty.GetValue(item)?.ToString();
//            var utfall = Convert.ToInt32(utfallProperty.GetValue(item));

//            // Logga värdet för att felsöka
//            Console.WriteLine($"SvarsalternativText: {svarsalternativText}, Utfall: {utfall}");

//            // Kontrollera om svarsalternativText är null
//            Assert.False(string.IsNullOrEmpty(svarsalternativText), "SvarsalternativText should not be null or empty");

//            // Förhindra null-nycklar
//            if (!string.IsNullOrEmpty(svarsalternativText))
//            {
//                Assert.True(expectedResults.ContainsKey(svarsalternativText), $"Unexpected SvarsalternativText: {svarsalternativText}");
//                Assert.Equal(expectedResults[svarsalternativText], utfall);
//            }
//        }


//    }
//    [Fact]
//    public async Task GetForskoleverksamheter_ReturnsOkResult_WithValidYear()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDb1");
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "Förskola A" });
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "Förskola B" });
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "Förskola A" }); // Duplicate, should be ignored by Distinct()
//        await context.SaveChangesAsync();

//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetForskoleverksamheter(2020);

//        // Assert
//        var okResult = Assert.IsType<OkObjectResult>(result);
//        var returnValue = Assert.IsType<List<string>>(okResult.Value);

//        // Kontrollera att rätt antal unika förskoleverksamheter returneras
//        Assert.Equal(2, returnValue.Count);
//        Assert.Contains("Förskola A", returnValue);
//        Assert.Contains("Förskola B", returnValue);
//    }

//    [Fact]
//    public async Task GetForskoleverksamheter_ReturnsBadRequest_WithInvalidYear()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDb2");
//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetForskoleverksamheter(2019); // Invalid year

//        // Assert
//        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//        Assert.Equal("Ogiltigt år.", badRequestResult.Value);
//    }

//    [Fact]
//    public async Task GetForskoleverksamheter_ReturnsEmptyList_WhenNoDataExists()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDb3");
//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetForskoleverksamheter(2020);

//        // Assert
//        var okResult = Assert.IsType<OkObjectResult>(result);
//        var returnValue = Assert.IsType<List<string>>(okResult.Value);

//        // Kontrollera att listan är tom
//        Assert.Empty(returnValue);
//    }
//    [Fact]
//    public async Task GetSurveyResponses_ReturnsBadRequest_WhenYearIsInvalid()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDbInvalidYear");
//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetSurveyResponses(2019, "XYZ", "ABC"); // Invalid year

//        // Assert
//        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//        Assert.Equal("Ogiltigt år.", badRequestResult.Value);
//    }
//    [Fact]
//    public async Task GetNöjdResponses_ReturnsBadRequest_WhenBothFragetextAndFrageNrAreNull()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDbBothNull");
//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetNöjdResponses(2020, "XYZ", null, null);

//        // Assert
//        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//        Assert.Equal("Antingen Frågetext eller FrageNr måste anges.", badRequestResult.Value);
//    }
//    [Fact]
//    public async Task GetNöjdResponses_ReturnsOk_WhenOnlyFragetextIsProvided()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDbOnlyFragetext");
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "XYZ", Fragetext = "ABC", SvarsalternativText = "3", Utfall = "10" });
//        await context.SaveChangesAsync();

//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetNöjdResponses(2020, "XYZ", "ABC");

//        // Assert
//        var okResult = Assert.IsType<OkObjectResult>(result);

//        // Använd System.Text.Json för att deserialisera resultatet
//        var jsonString = JsonSerializer.Serialize(okResult.Value);
//        var returnValue = JsonSerializer.Deserialize<Dictionary<string, int>>(jsonString);

//        Assert.NotNull(returnValue);
//        Assert.Equal(10, returnValue["AntalNöjdaSvar"]);  // Förväntar sig en nöjd svar
//        Assert.Equal(10, returnValue["TotaltAntalSvar"]); // Förväntar sig ett totalt svar
//    }
//    [Fact]
//    public async Task GetSvarsalternativResponses_ReturnsBadRequest_WhenForskoleverksamhetIsMissing()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDbMissingForskoleverksamhet");
//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetSvarsalternativResponses(2020, null, "ABC", "25");

//        // Assert
//        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//        Assert.Equal("Förskoleverksamhet måste anges.", badRequestResult.Value);
//    }
//    [Fact]
//    public async Task GetSvarsalternativResponses_ReturnsBadRequest_WhenYearIsInvalid()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDbInvalidYear");
//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetSvarsalternativResponses(2019, "XYZ", "ABC", "25");

//        // Assert
//        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//        Assert.Equal("Ogiltigt år.", badRequestResult.Value);
//    }
//    [Fact]
//    public async Task GetSvarsalternativResponses_ReturnsOk_WithEmptyList_WhenNoDataMatches()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDbNoMatchingData");
//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetSvarsalternativResponses(2020, "NonExistent", "NoMatch", "99");

//        // Assert
//        var okResult = Assert.IsType<OkObjectResult>(result);

//        // Kontrollera om resultatet är null och hantera det på rätt sätt
//        Assert.NotNull(okResult.Value);
//        var returnValue = okResult.Value as List<object>;

//        Assert.NotNull(returnValue);
//        Assert.Empty(returnValue); // Kontrollera att listan är tom
//    }

//    [Fact]
//    public async Task GetFragetexter_ReturnsOk_WithValidYear()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDbFragetexterValidYear");
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "XYZ", Fragetext = "ABC" });
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "XYZ", Fragetext = "DEF" });
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "XYZ", Fragetext = "ABC" }); // Duplicera, ska ignoreras av Distinct()
//        await context.SaveChangesAsync();

//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetFragetexter(2020);

//        // Assert
//        var okResult = Assert.IsType<OkObjectResult>(result);
//        var returnValue = Assert.IsType<List<string>>(okResult.Value);

//        Assert.NotNull(returnValue);
//        Assert.Equal(2, returnValue.Count); // Förväntar sig 2 distinkta fragetexter
//        Assert.Contains("ABC", returnValue);
//        Assert.Contains("DEF", returnValue);
//    }
//    [Fact]
//    public async Task GetFragetexter_ReturnsBadRequest_WithInvalidYear()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDbFragetexterInvalidYear");
//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetFragetexter(2019); // Ogiltigt år

//        // Assert
//        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//        Assert.Equal("Ogiltigt år.", badRequestResult.Value);
//    }
//    [Fact]
//    public async Task GetFragetexter_ReturnsOk_WithEmptyList_WhenNoDataExists()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDbFragetexterNoData");
//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetFragetexter(2020); // Inget data för detta år

//        // Assert
//        var okResult = Assert.IsType<OkObjectResult>(result);
//        var returnValue = Assert.IsType<List<string>>(okResult.Value);

//        Assert.NotNull(returnValue);
//        Assert.Empty(returnValue); // Kontrollera att listan är tom
//    }
//    [Fact]
//    public async Task GetSurveyResponseByName_ReturnsOk_WithValidName()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDbByNameValid");
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "Förskolan ABC" });
//        await context.SaveChangesAsync();

//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetSurveyResponseByName("Förskolan ABC");

//        // Assert
//        var actionResult = Assert.IsType<ActionResult<IEnumerable<SurveyResponse>>>(result);
//        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

//        var returnValue = Assert.IsType<List<SurveyResponse>>(okResult.Value);

//        Assert.NotNull(returnValue);
//        Assert.Single(returnValue);
//        Assert.Equal("Förskolan ABC", returnValue[0].Forskoleverksamhet);
//    }

//    [Fact]
//    public async Task GetSurveyResponseByName_ReturnsOk_WithNameContainingMultipleWords()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDbByNameMultipleWords");
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "Föräldrakooperativet Lilla Äventyret" });
//        await context.SaveChangesAsync();

//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetSurveyResponseByName("Lilla Äventyret");

//        // Assert
//        var actionResult = Assert.IsType<ActionResult<IEnumerable<SurveyResponse>>>(result);
//        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

//        var returnValue = Assert.IsType<List<SurveyResponse>>(okResult.Value);
//        Assert.NotNull(returnValue);
//        Assert.Single(returnValue);
//        Assert.Equal("Föräldrakooperativet Lilla Äventyret", returnValue[0].Forskoleverksamhet);
//    }


//    [Fact]
//    public async Task GetSurveyResponseByName_ReturnsOk_WithPartialMatches()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDbByNamePartialMatch");
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "Förskolan Lärkan" });
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "Föräldrakooperativet Lärkan" });
//        await context.SaveChangesAsync();

//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetSurveyResponseByName("Lärkan");

//        // Assert
//        var actionResult = Assert.IsType<ActionResult<IEnumerable<SurveyResponse>>>(result);
//        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

//        var returnValue = Assert.IsType<List<SurveyResponse>>(okResult.Value);

//        Assert.NotNull(returnValue);
//        Assert.Equal(2, returnValue.Count);  // Förväntar sig 2 matchningar
//        Assert.Contains(returnValue, r => r.Forskoleverksamhet == "Förskolan Lärkan");
//        Assert.Contains(returnValue, r => r.Forskoleverksamhet == "Föräldrakooperativet Lärkan");
//    }
//    [Fact]
//    public async Task GetSatisfactionSummary_ReturnsOk_WithValidData()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDbSatisfactionSummaryValidData");
//        context.SurveyResponses.Add(new SurveyResponse
//        {
//            Forskoleverksamhet = "Förskola ABC",
//            AvserAr = "2023",
//            Fragetext = "Jag är som helhet nöjd med mitt barns förskola",
//            SvarsalternativText = "Instämmer helt"
//        });
//        context.SurveyResponses.Add(new SurveyResponse
//        {
//            Forskoleverksamhet = "Förskola ABC",
//            AvserAr = "2023",
//            Fragetext = "Jag är som helhet nöjd med mitt barns förskola",
//            SvarsalternativText = "Instämmer i stor utsträckning"
//        });
//        context.SurveyResponses.Add(new SurveyResponse
//        {
//            Forskoleverksamhet = "Förskola XYZ",
//            AvserAr = "2023",
//            Fragetext = "Jag är som helhet nöjd med mitt barns förskola",
//            SvarsalternativText = "Instämmer helt"
//        });
//        await context.SaveChangesAsync();

//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetSatisfactionSummary();

//        // Assert
//        var actionResult = Assert.IsType<ActionResult<IEnumerable<SatisfactionSummary>>>(result);
//        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

//        var returnValue = Assert.IsType<List<SatisfactionSummary>>(okResult.Value);

//        Assert.NotNull(returnValue);
//        Assert.Equal(2, returnValue.Count);  // Förväntar sig 2 distinkta sammanfattningar

//        var satisfactionABC = returnValue.FirstOrDefault(x => x.Forskoleverksamhet == "Förskola ABC" && x.Year == "2023");
//        Assert.NotNull(satisfactionABC);
//        Assert.Equal(2, satisfactionABC.PositiveResponses);  // Förväntar sig 2 positiva svar för "Förskola ABC"
//        Assert.Equal(2, satisfactionABC.TotalResponses);  // Förväntar sig totalt 2 svar för "Förskola ABC"

//        var satisfactionXYZ = returnValue.FirstOrDefault(x => x.Forskoleverksamhet == "Förskola XYZ" && x.Year == "2023");
//        Assert.NotNull(satisfactionXYZ);
//        Assert.Equal(1, satisfactionXYZ.PositiveResponses);  // Förväntar sig 1 positivt svar för "Förskola XYZ"
//        Assert.Equal(1, satisfactionXYZ.TotalResponses);  // Förväntar sig totalt 1 svar för "Förskola XYZ"
//    }
//    [Fact]
//    public async Task GetSatisfactionSummary_ReturnsOk_WithEmptyList_WhenNoDataExists()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDbSatisfactionSummaryNoData");
//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetSatisfactionSummary();

//        // Assert
//        var actionResult = Assert.IsType<ActionResult<IEnumerable<SatisfactionSummary>>>(result);
//        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

//        var returnValue = Assert.IsType<List<SatisfactionSummary>>(okResult.Value);

//        Assert.NotNull(returnValue);
//        Assert.Empty(returnValue);  // Kontrollera att listan är tom när ingen data matchar
//    }

//}
