using Backend.Interfaces;
using Backend.Models;
using Backend.Services;
using MongoDB.Driver;
using Moq;

namespace BackendTest;

public class RectangleRepositoryTests
{
    private readonly Mock<IMongoService> _mockMongoService;
    private readonly Mock<IMongoCollection<Rectangle>> _mockCollection;
    private readonly Mock<IAsyncCursor<Rectangle>> _mockCursor;
    private readonly RectangleService _sut; // System Under Test

    public RectangleRepositoryTests()
    {
        _mockMongoService = new Mock<IMongoService>();
        _mockCollection = new Mock<IMongoCollection<Rectangle>>();
        _mockCursor = new Mock<IAsyncCursor<Rectangle>>();

        SetupMongoCollection();
        _sut = new RectangleService(_mockMongoService.Object);
    }

    // Testing to get all rectangles from the service and return them as a list of rectangles.
    [Theory]
    [InlineData("1", "red", 0, 0)]
    [InlineData("2", "blue", 10, 10)]
    public async Task GetAllRectangles_WhenCalled_ReturnsStoredRectangles(string id, string color, int x, int y)
    {
        // Arrange
        var expectedRectangle = CreateTestRectangle(id, color, x, y);
        SetupMongoCursor(new List<Rectangle> { expectedRectangle });

        // Act
        var result = await _sut.GetAll();

        // Assert
        Assert.Contains(result, rectangle =>
            rectangle.Id == id &&
            rectangle.Color == color &&
            rectangle.X == x &&
            rectangle.Y == y);
    }

    // Testing to add a rectangle to the service and return the created rectangle.
    [Fact]
    public async Task AddRectangle_WhenValidInput_StoresAndReturnsRectangle()
    {
        // Arrange
        var rectangle = CreateTestRectangle("test", "green", 5, 5);

        // Act
        var result = await _sut.Add(rectangle);

        // Assert
        Assert.Equal(rectangle, result);
        VerifyRectangleWasStored(rectangle);
    }

    // Testing to delete all rectangles from the service.
    [Fact]
    public async Task DeleteAllRectangles_WhenCalled_ClearsAllStoredRectangles()
    {
        // Act
        await _sut.DeleteAll();

        // Assert
        VerifyAllRectanglesWereDeleted();
    }

    // ** Helper methods.

    // Setup the mock to return the collection when requested.
    private void SetupMongoCollection()
    {
        _mockMongoService.Setup(service => service.GetCollection<Rectangle>("rectangles"))
            .Returns(_mockCollection.Object);
    }

    // Setup the mock cursor to return the specified rectangles.
    private void SetupMongoCursor(List<Rectangle> rectangles)
    {
        _mockCursor.Setup(c => c.Current).Returns(rectangles);
        _mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(false);
        _mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        _mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Rectangle>>(),
                It.IsAny<FindOptions<Rectangle>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockCursor.Object);
    }

    // Create a test rectangle with the specified properties.
    private Rectangle CreateTestRectangle(string id, string color, int x, int y)
    {
        return new Rectangle { Id = id, Color = color, X = x, Y = y };
    }

    // Verify that the rectangle was stored in the collection.
    private void VerifyRectangleWasStored(Rectangle rectangle)
    {
        _mockCollection.Verify(c => c.InsertOneAsync(
                It.Is<Rectangle>(r =>
                    r.Color == rectangle.Color &&
                    r.X == rectangle.X &&
                    r.Y == rectangle.Y),
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    // Verify that all rectangles were deleted from the collection.
    private void VerifyAllRectanglesWereDeleted()
    {
        _mockCollection.Verify(c => c.DeleteManyAsync(
                It.IsAny<FilterDefinition<Rectangle>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}