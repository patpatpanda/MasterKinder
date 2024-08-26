
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

//        // Logga antalet poster som returneras f�r fels�kning
//        Console.WriteLine($"Antal poster returnerade: {returnValue.Count}");

//        // Kontrollera att en post returneras
//        Assert.Single(returnValue);  // F�rv�ntar sig en enda post
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
//        Assert.Equal("F�rskoleverksamhet och Fr�getext m�ste anges.", badRequestResult.Value);
//    }



//    [Fact]
//    public async Task GetN�jdResponses_ReturnsOkResult_WithValidParameters()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDb3");
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "XYZ", Fragetext = "ABC", SvarsalternativText = "3", Utfall = "10" });
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "XYZ", Fragetext = "ABC", SvarsalternativText = "1", Utfall = "5" });
//        await context.SaveChangesAsync();

//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetN�jdResponses(2020, "XYZ", "ABC");

//        // Assert
//        var okResult = Assert.IsType<OkObjectResult>(result);

//        // Casta det returnerade v�rdet till en k�nd anonym typ (det h�r fungerar bara om du vet exakt hur resultatet ser ut)
//        var expectedType = new { AntalN�jdaSvar = 0, TotaltAntalSvar = 0 };
//        var returnValue = okResult.Value.GetType().GetProperties()
//            .ToDictionary(p => p.Name, p => p.GetValue(okResult.Value));

//        Assert.NotNull(returnValue);
//        Assert.Equal(10, returnValue["AntalN�jdaSvar"]);
//        Assert.Equal(15, returnValue["TotaltAntalSvar"]);
//    }

    


//    [Fact]
//    public async Task GetN�jdResponses_ReturnsBadRequest_WhenForskoleverksamhetIsMissing()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDb4");
//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetN�jdResponses(2020, null, "ABC");

//        // Assert
//        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//        Assert.Equal("F�rskoleverksamhet m�ste anges.", badRequestResult.Value);
//    }

//    [Fact]
//    public async Task GetN�jdResponses_ReturnsBadRequest_WhenFragetextAndFrageNrAreMissing()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDb5");
//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetN�jdResponses(2020, "XYZ", null, null);

//        // Assert
//        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//        Assert.Equal("Antingen Fr�getext eller FrageNr m�ste anges.", badRequestResult.Value);
//    }

//    [Fact]
//    public async Task GetN�jdResponses_ReturnsBadRequest_WhenYearIsInvalid()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDb6");
//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetN�jdResponses(2019, "XYZ", "ABC");

//        // Assert
//        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//        Assert.Equal("Ogiltigt �r.", badRequestResult.Value);
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

//        // Kontrollera att returnerat v�rde �r en lista
//        var returnValue = okResult.Value as IEnumerable<object>;
//        Assert.NotNull(returnValue);

//        // F�rv�ntade svar
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

//            // Logga v�rdet f�r att fels�ka
//            Console.WriteLine($"SvarsalternativText: {svarsalternativText}, Utfall: {utfall}");

//            // Kontrollera om svarsalternativText �r null
//            Assert.False(string.IsNullOrEmpty(svarsalternativText), "SvarsalternativText should not be null or empty");

//            // F�rhindra null-nycklar
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
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "F�rskola A" });
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "F�rskola B" });
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "F�rskola A" }); // Duplicate, should be ignored by Distinct()
//        await context.SaveChangesAsync();

//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetForskoleverksamheter(2020);

//        // Assert
//        var okResult = Assert.IsType<OkObjectResult>(result);
//        var returnValue = Assert.IsType<List<string>>(okResult.Value);

//        // Kontrollera att r�tt antal unika f�rskoleverksamheter returneras
//        Assert.Equal(2, returnValue.Count);
//        Assert.Contains("F�rskola A", returnValue);
//        Assert.Contains("F�rskola B", returnValue);
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
//        Assert.Equal("Ogiltigt �r.", badRequestResult.Value);
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

//        // Kontrollera att listan �r tom
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
//        Assert.Equal("Ogiltigt �r.", badRequestResult.Value);
//    }
//    [Fact]
//    public async Task GetN�jdResponses_ReturnsBadRequest_WhenBothFragetextAndFrageNrAreNull()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDbBothNull");
//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetN�jdResponses(2020, "XYZ", null, null);

//        // Assert
//        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//        Assert.Equal("Antingen Fr�getext eller FrageNr m�ste anges.", badRequestResult.Value);
//    }
//    [Fact]
//    public async Task GetN�jdResponses_ReturnsOk_WhenOnlyFragetextIsProvided()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDbOnlyFragetext");
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "XYZ", Fragetext = "ABC", SvarsalternativText = "3", Utfall = "10" });
//        await context.SaveChangesAsync();

//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetN�jdResponses(2020, "XYZ", "ABC");

//        // Assert
//        var okResult = Assert.IsType<OkObjectResult>(result);

//        // Anv�nd System.Text.Json f�r att deserialisera resultatet
//        var jsonString = JsonSerializer.Serialize(okResult.Value);
//        var returnValue = JsonSerializer.Deserialize<Dictionary<string, int>>(jsonString);

//        Assert.NotNull(returnValue);
//        Assert.Equal(10, returnValue["AntalN�jdaSvar"]);  // F�rv�ntar sig en n�jd svar
//        Assert.Equal(10, returnValue["TotaltAntalSvar"]); // F�rv�ntar sig ett totalt svar
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
//        Assert.Equal("F�rskoleverksamhet m�ste anges.", badRequestResult.Value);
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
//        Assert.Equal("Ogiltigt �r.", badRequestResult.Value);
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

//        // Kontrollera om resultatet �r null och hantera det p� r�tt s�tt
//        Assert.NotNull(okResult.Value);
//        var returnValue = okResult.Value as List<object>;

//        Assert.NotNull(returnValue);
//        Assert.Empty(returnValue); // Kontrollera att listan �r tom
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
//        Assert.Equal(2, returnValue.Count); // F�rv�ntar sig 2 distinkta fragetexter
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
//        var result = await controller.GetFragetexter(2019); // Ogiltigt �r

//        // Assert
//        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//        Assert.Equal("Ogiltigt �r.", badRequestResult.Value);
//    }
//    [Fact]
//    public async Task GetFragetexter_ReturnsOk_WithEmptyList_WhenNoDataExists()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDbFragetexterNoData");
//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetFragetexter(2020); // Inget data f�r detta �r

//        // Assert
//        var okResult = Assert.IsType<OkObjectResult>(result);
//        var returnValue = Assert.IsType<List<string>>(okResult.Value);

//        Assert.NotNull(returnValue);
//        Assert.Empty(returnValue); // Kontrollera att listan �r tom
//    }
//    [Fact]
//    public async Task GetSurveyResponseByName_ReturnsOk_WithValidName()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDbByNameValid");
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "F�rskolan ABC" });
//        await context.SaveChangesAsync();

//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetSurveyResponseByName("F�rskolan ABC");

//        // Assert
//        var actionResult = Assert.IsType<ActionResult<IEnumerable<SurveyResponse>>>(result);
//        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

//        var returnValue = Assert.IsType<List<SurveyResponse>>(okResult.Value);

//        Assert.NotNull(returnValue);
//        Assert.Single(returnValue);
//        Assert.Equal("F�rskolan ABC", returnValue[0].Forskoleverksamhet);
//    }

//    [Fact]
//    public async Task GetSurveyResponseByName_ReturnsOk_WithNameContainingMultipleWords()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDbByNameMultipleWords");
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "F�r�ldrakooperativet Lilla �ventyret" });
//        await context.SaveChangesAsync();

//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetSurveyResponseByName("Lilla �ventyret");

//        // Assert
//        var actionResult = Assert.IsType<ActionResult<IEnumerable<SurveyResponse>>>(result);
//        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

//        var returnValue = Assert.IsType<List<SurveyResponse>>(okResult.Value);
//        Assert.NotNull(returnValue);
//        Assert.Single(returnValue);
//        Assert.Equal("F�r�ldrakooperativet Lilla �ventyret", returnValue[0].Forskoleverksamhet);
//    }


//    [Fact]
//    public async Task GetSurveyResponseByName_ReturnsOk_WithPartialMatches()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDbByNamePartialMatch");
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "F�rskolan L�rkan" });
//        context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "F�r�ldrakooperativet L�rkan" });
//        await context.SaveChangesAsync();

//        var controller = new SurveyController(context);

//        // Act
//        var result = await controller.GetSurveyResponseByName("L�rkan");

//        // Assert
//        var actionResult = Assert.IsType<ActionResult<IEnumerable<SurveyResponse>>>(result);
//        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

//        var returnValue = Assert.IsType<List<SurveyResponse>>(okResult.Value);

//        Assert.NotNull(returnValue);
//        Assert.Equal(2, returnValue.Count);  // F�rv�ntar sig 2 matchningar
//        Assert.Contains(returnValue, r => r.Forskoleverksamhet == "F�rskolan L�rkan");
//        Assert.Contains(returnValue, r => r.Forskoleverksamhet == "F�r�ldrakooperativet L�rkan");
//    }
//    [Fact]
//    public async Task GetSatisfactionSummary_ReturnsOk_WithValidData()
//    {
//        // Arrange
//        var context = GetInMemoryDbContext("TestDbSatisfactionSummaryValidData");
//        context.SurveyResponses.Add(new SurveyResponse
//        {
//            Forskoleverksamhet = "F�rskola ABC",
//            AvserAr = "2023",
//            Fragetext = "Jag �r som helhet n�jd med mitt barns f�rskola",
//            SvarsalternativText = "Inst�mmer helt"
//        });
//        context.SurveyResponses.Add(new SurveyResponse
//        {
//            Forskoleverksamhet = "F�rskola ABC",
//            AvserAr = "2023",
//            Fragetext = "Jag �r som helhet n�jd med mitt barns f�rskola",
//            SvarsalternativText = "Inst�mmer i stor utstr�ckning"
//        });
//        context.SurveyResponses.Add(new SurveyResponse
//        {
//            Forskoleverksamhet = "F�rskola XYZ",
//            AvserAr = "2023",
//            Fragetext = "Jag �r som helhet n�jd med mitt barns f�rskola",
//            SvarsalternativText = "Inst�mmer helt"
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
//        Assert.Equal(2, returnValue.Count);  // F�rv�ntar sig 2 distinkta sammanfattningar

//        var satisfactionABC = returnValue.FirstOrDefault(x => x.Forskoleverksamhet == "F�rskola ABC" && x.Year == "2023");
//        Assert.NotNull(satisfactionABC);
//        Assert.Equal(2, satisfactionABC.PositiveResponses);  // F�rv�ntar sig 2 positiva svar f�r "F�rskola ABC"
//        Assert.Equal(2, satisfactionABC.TotalResponses);  // F�rv�ntar sig totalt 2 svar f�r "F�rskola ABC"

//        var satisfactionXYZ = returnValue.FirstOrDefault(x => x.Forskoleverksamhet == "F�rskola XYZ" && x.Year == "2023");
//        Assert.NotNull(satisfactionXYZ);
//        Assert.Equal(1, satisfactionXYZ.PositiveResponses);  // F�rv�ntar sig 1 positivt svar f�r "F�rskola XYZ"
//        Assert.Equal(1, satisfactionXYZ.TotalResponses);  // F�rv�ntar sig totalt 1 svar f�r "F�rskola XYZ"
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
//        Assert.Empty(returnValue);  // Kontrollera att listan �r tom n�r ingen data matchar
//    }

//}
