using Insttantt.FlowManagement.Api.Controllers;
using Insttantt.FlowManagement.Application.Common.Interfaces.Services;
using Insttantt.FlowManagement.Application.Middleware;
using Insttantt.FlowManagement.Domain.Entities;
using Insttantt.FlowManagement.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Insttantt.FlowManagement.UnitTest.Controllers
{
    public class FlowControllerTest
    {
        private readonly Mock<ILogger<ExceptionHandler>> _mocklogger;
        private readonly Mock<IFlowService> _mockFlowService;

        public FlowControllerTest()
        {
            _mocklogger = new Mock<ILogger<ExceptionHandler>>();
            _mockFlowService = new Mock<IFlowService>();
        }

        [Fact]
        public async Task GetAllFlows_Returns_OkObjectResult_When_Service_Successful()
        {
            // Arrange
            _mockFlowService.Setup(service => service.GetAllFlowAsync())
                .ReturnsAsync(new List<Flow>()); 

            var controller = new FlowsController(_mockFlowService.Object, _mocklogger.Object);

            // Act
            var result = await controller.GetAllFlows();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<Flow>>(okResult.Value);
            Assert.Empty(model); // Check if returned list is empty
        }

        [Fact]
        public async Task GetAllFlows_Returns_BadRequest_When_Service_Fails()
        {
            // Arrange
            _mockFlowService.Setup(service => service.GetAllFlowAsync())
                .ThrowsAsync(new Exception("Some error occurred"));

            var controller = new FlowsController(_mockFlowService.Object, _mocklogger.Object);

            // Act
            var result = await controller.GetAllFlows();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Some error occurred", badRequestResult.Value);
        }

        [Fact]
        public async Task AddFlow_Returns_OkObjectResult_When_Service_Successful()
        {
            // Arrange
            var flowRequest = new FlowRequest();
            var mockFlow = new Flow();
            _mockFlowService.Setup(service => service.AddFlowAsync(flowRequest))
                .ReturnsAsync(mockFlow); 

            var controller = new FlowsController(_mockFlowService.Object, _mocklogger.Object);

            // Act
            var result = await controller.AddFlow(flowRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsType<Flow>(okResult.Value);
            Assert.Same(mockFlow, model);
        }

        [Fact]
        public async Task AddFlow_Returns_BadRequest_When_Service_Fails()
        {
            // Arrange
            var flowRequest = new FlowRequest(); // Provide necessary data for request
            _mockFlowService.Setup(service => service.AddFlowAsync(flowRequest))
                .ThrowsAsync(new Exception("Some error occurred"));

            var mockLogger = new Mock<ILogger<ExceptionHandler>>();

            var controller = new FlowsController(_mockFlowService.Object, _mocklogger.Object);

            // Act
            var result = await controller.AddFlow(flowRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("An error occurred while creating the flow.", badRequestResult.Value);
        }
    }
}
