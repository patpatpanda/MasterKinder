using MasterKinder.Controllers;
using MasterKinder.Data;
using MasterKinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MasterKinder.Services;
using System.Net.Http;
using Moq;
using Moq.Protected;
using System.Threading;
using System.Net;

public class ForskolanControllerIntegrationTests
{
    private MrDb GetInMemoryDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<MrDb>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        var context = new MrDb(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return context;
    }

    private HttpClient GetMockedHttpClient()
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        handlerMock
           .Protected()
           // Setup the PROTECTED method to mock
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>()
           )
           // prepare the expected response of the mocked HttpClient
           .ReturnsAsync(new HttpResponseMessage()
           {
               StatusCode = HttpStatusCode.OK,
               Content = new StringContent("{'results':[{'geometry':{'lat':58.0,'lng':11.0}}]}")
           })
           .Verifiable();

        // use real http client with mocked handler here
        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new System.Uri("https://api.opencagedata.com")
        };

        return httpClient;
    }

    [Fact]
    public async Task GetForskolans_ReturnsOkResult_WithListOfForskolans()
    {
        // Arrange
        var context = GetInMemoryDbContext("TestDb1");
        context.Forskolans.AddRange(new List<Forskolan>
        {
            new Forskolan { Id = 1, Namn = "Förskola A", Adress = "Adress A" },
            new Forskolan { Id = 2, Namn = "Förskola B", Adress = "Adress B" }
        });
        await context.SaveChangesAsync();

        var logger = new LoggerFactory().CreateLogger<ForskolanController>();
        var mockedHttpClient = GetMockedHttpClient();
        var geocodeService = new GeocodeService(mockedHttpClient);

        var controller = new ForskolanController(context, geocodeService, logger);

        // Act
        var result = await controller.GetForskolans();

        // Assert
        var okResult = Assert.IsType<ActionResult<IEnumerable<Forskolan>>>(result);
        var returnValue = Assert.IsType<List<Forskolan>>(okResult.Value);

        Assert.Equal(2, returnValue.Count);
        Assert.Equal("Förskola A", returnValue[0].Namn);
        Assert.Equal("Förskola B", returnValue[1].Namn);
    }

    [Fact]
    public async Task GetForskolans_ReturnsOkResult_WithEmptyList_WhenNoForskolansExist()
    {
        // Arrange
        var context = GetInMemoryDbContext("TestDb2");
        var logger = new LoggerFactory().CreateLogger<ForskolanController>();
        var mockedHttpClient = GetMockedHttpClient();
        var geocodeService = new GeocodeService(mockedHttpClient);

        var controller = new ForskolanController(context, geocodeService, logger);

        // Act
        var result = await controller.GetForskolans();

        // Assert
        var okResult = Assert.IsType<ActionResult<IEnumerable<Forskolan>>>(result);
        var returnValue = Assert.IsType<List<Forskolan>>(okResult.Value);
        Assert.Empty(returnValue);
    }
}
