//using MasterKinder.Data;
//using MasterKinder.Models;
//using MasterKinderAPI.Controllers;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace KinderTest
//{
//    public class Dolcevita
//    {
//        private MrDb GetInMemoryDbContext(string dbName)
//        {
//            var options = new DbContextOptionsBuilder<MrDb>()
//                .UseInMemoryDatabase(databaseName: dbName)
//                .Options;

//            var context = new MrDb(options);

//            // Ensure the database is deleted to start fresh each time
//            context.Database.EnsureDeleted();

//            return context;
//        }

//        [Fact]
//        public async Task GetForskoleverksamheter_ReturnsOkResult_WithValidYear()
//        {
//            // Arrange
//            var context = GetInMemoryDbContext("TestDb1");
//            context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "Förskola A" });
//            context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "Förskola B" });
//            context.SurveyResponses.Add(new SurveyResponse { Forskoleverksamhet = "Förskola A" }); // Duplicate, should be ignored by Distinct()
//            await context.SaveChangesAsync();

//            var controller = new SurveyController(context);

//            // Act
//            var result = await controller.GetForskoleverksamheter(2020);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            var returnValue = Assert.IsType<List<string>>(okResult.Value);

//            // Kontrollera att rätt antal unika förskoleverksamheter returneras
//            Assert.Equal(2, returnValue.Count);
//            Assert.Contains("Förskola A", returnValue);
//            Assert.Contains("Förskola B", returnValue);
//        }

//        [Fact]
//        public async Task GetForskoleverksamheter_ReturnsBadRequest_WithInvalidYear()
//        {
//            // Arrange
//            var context = GetInMemoryDbContext("TestDb2");
//            var controller = new SurveyController(context);

//            // Act
//            var result = await controller.GetForskoleverksamheter(2019); // Invalid year

//            // Assert
//            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//            Assert.Equal("Ogiltigt år.", badRequestResult.Value);
//        }

//        [Fact]
//        public async Task GetForskoleverksamheter_ReturnsEmptyList_WhenNoDataExists()
//        {
//            // Arrange
//            var context = GetInMemoryDbContext("TestDb3");
//            var controller = new SurveyController(context);

//            // Act
//            var result = await controller.GetForskoleverksamheter(2020);

//            // Assert
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            var returnValue = Assert.IsType<List<string>>(okResult.Value);

//            // Kontrollera att listan är tom
//            Assert.Empty(returnValue);
//        }
//    }
//}
