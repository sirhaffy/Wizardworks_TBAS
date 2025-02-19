using Backend.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace BackendTest;

public class MongoConnectionConfigurationTests
{
    private readonly Mock<IConfiguration> _mockConfiguration = new();
    private readonly Mock<ILogger<MongoService>> _mockLogger = new();

    // Testing to connect to MongoDB with valid configuration.
    [Fact]
    public void Connect_WithValidConfiguration_ShouldSucceed()
    {
        // Arrange
        var host = Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true" 
            ? "mongodb" 
            : "localhost";
            
        Environment.SetEnvironmentVariable("MONGODB_HOST", host);
        SetupConfiguration($"mongodb://{host}:27017", "testdb");

        // Act
        var service = new MongoService(_mockConfiguration.Object, _mockLogger.Object);

        // Assert
        Assert.NotNull(service);
    }

    [Theory]
    [InlineData(null, "testdb", "connectionString")]
    [InlineData("mongodb://localhost:27017", null, "databaseName")]
    public void Connect_WithInvalidConfiguration_ShouldThrowException(
        string? connectionString,
        string? databaseName,
        string expectedParamName)
    {
        // Arrange
        SetupConfiguration(connectionString, databaseName);

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(
            () => new MongoService(_mockConfiguration.Object, _mockLogger.Object));
        
        Assert.Equal(expectedParamName, exception.ParamName);
    }

    private void SetupConfiguration(string? connectionString, string? databaseName)
    {
        // Setup för Connection String
        var connectionSection = new Mock<IConfigurationSection>();
        connectionSection.Setup(x => x.Value).Returns(connectionString);
        _mockConfiguration
            .Setup(x => x.GetSection("MongoDB:ConnectionString"))
            .Returns(connectionSection.Object);
        
        // Setup för Database Name
        var databaseSection = new Mock<IConfigurationSection>();
        databaseSection.Setup(x => x.Value).Returns(databaseName);
        _mockConfiguration
            .Setup(x => x.GetSection("MongoDB:DatabaseName"))
            .Returns(databaseSection.Object);
    }
}