using Backend.Controller;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BackendTest;

// Tests the RectangleController's HTTP endpoints for managing rectangles
public class RectangleApiEndpointTests : IDisposable
{
    private readonly Mock<IRectangleService> _mockRectangleService;
    private readonly Mock<ILogger<RectangleController>> _mockLogger;
    private readonly RectangleController _sut;
    private readonly Rectangle _testRectangle;

    public RectangleApiEndpointTests()
    {
        _mockRectangleService = new Mock<IRectangleService>();
        _mockLogger = new Mock<ILogger<RectangleController>>();
        _sut = new RectangleController(_mockRectangleService.Object, _mockLogger.Object);
        _testRectangle = CreateTestRectangle();

        SetupDefaultServiceBehavior();
    }

    // Testing to get all rectangles from the service and return them as a list of rectangles.
    [Theory]
    [InlineData("1", "red", 0, 0)]
    [InlineData("2", "blue", 10, 10)]
    [InlineData("3", "yellow", 20, 20)]
    [InlineData("4", "green", 30, 30)]
    public async Task GetEndpoint_WhenCalled_ReturnsExpectedRectangles(string id, string color, int x, int y)
    {
        // Arrange
        var expectedRectangle = new Rectangle { Id = id, Color = color, X = x, Y = y };
        SetupGetAllResponse(new List<Rectangle> { expectedRectangle });

        // Act
        var result = await _sut.GetAll();

        // Assert
        VerifyHttpResponse(result, expectedRectangle);
    }

    // Testing to add a rectangle to the service and return the created rectangle.
    [Fact]
    public async Task AddEndpoint_WithValidRectangle_ReturnsCreatedRectangle()
    {
        // Act
        var result = await _sut.Add(_testRectangle);

        // Assert
        VerifyRectangleResponse(result);
        VerifyOperationWasLogged();
    }

    // Testing to delete all rectangles from the service.
    [Fact]
    public async Task DeleteEndpoint_WhenCalled_ReturnsNoContent()
    {
        // Arrange
        SetupDeleteAllResponse();

        // Act
        var result = await _sut.DeleteAll();

        // Assert
        Assert.IsType<NoContentResult>(result);
        VerifyOperationWasLogged();
    }

    // Testing to delete all rectangles from the service and return no content.
    public void Dispose()
    {
        _mockRectangleService.Reset();
        _mockLogger.Reset();
    }


    // ** Helper methods.

    // Creates a test rectangle.
    private Rectangle CreateTestRectangle()
    {
        return new Rectangle { Color = "green", X = 5, Y = 5 };
    }

    // Sets up the service to return the test rectangle when adding a rectangle.
    private void SetupDefaultServiceBehavior()
    {
        _mockRectangleService.Setup(service => service.Add(It.IsAny<Rectangle>()))
            .ReturnsAsync(_testRectangle);
    }

    // Sets up the service to return a list of rectangles when getting all rectangles.
    private void SetupGetAllResponse(List<Rectangle> rectangles)
    {
        _mockRectangleService.Setup(service => service.GetAll())
            .ReturnsAsync(rectangles);
    }

    // Sets up the service to return a successful response when deleting all rectangles.
    private void SetupDeleteAllResponse()
    {
        _mockRectangleService.Setup(service => service.DeleteAll())
            .Returns(Task.CompletedTask);
    }

    // Verifies that the HTTP response contains the expected rectangle.
    private void VerifyHttpResponse(ActionResult<List<Rectangle>> result, Rectangle expected)
    {
        var actionResult = Assert.IsType<ActionResult<List<Rectangle>>>(result);
        var rectangles = Assert.IsType<List<Rectangle>>(actionResult.Value);
        Assert.Single(rectangles);

        var rectangle = rectangles[0];
        Assert.Equal(expected.Id, rectangle.Id);
        Assert.Equal(expected.Color, rectangle.Color);
        Assert.Equal(expected.X, rectangle.X);
        Assert.Equal(expected.Y, rectangle.Y);
    }

    // Verifies that the HTTP response contains the expected rectangle.
    private void VerifyRectangleResponse(ActionResult<Rectangle> result)
    {
        var actionResult = Assert.IsType<ActionResult<Rectangle>>(result);
        var addedRectangle = Assert.IsType<Rectangle>(actionResult.Value);
        Assert.Equal(_testRectangle.Color, addedRectangle.Color);
        Assert.Equal(_testRectangle.X, addedRectangle.X);
        Assert.Equal(_testRectangle.Y, addedRectangle.Y);
    }

    // Verifies that the operation was logged.
    private void VerifyOperationWasLogged()
    {
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}



