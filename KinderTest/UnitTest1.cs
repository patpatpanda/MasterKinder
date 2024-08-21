using Xunit;
using Moq;
using MasterKinder.Controllers;
using MasterKinder.Data;
using MasterKinder.Services;
using Microsoft.Extensions.Logging;

public class ForskolanControllerTests
{
    [Fact]
    public void GetForskolan_ReturnsNotNull_WhenCalled()
    {
        // Arrange
        var mockDbContext = new Mock<MrDb>();  // Mocka MrDb
        var mockGeocodeService = new Mock<GeocodeService>();  
        var mockLogger = new Mock<ILogger<ForskolanController>>(); 

        var controller = new ForskolanController(
            mockDbContext.Object,
            mockGeocodeService.Object,
            mockLogger.Object
        );

        // Act
        var result = controller.GetForskolan(1);

        // Assert
        Assert.NotNull(result);
    }
}
