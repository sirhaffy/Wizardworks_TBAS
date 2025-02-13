using Backend.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace BackendTest;

public class MongoConnectionConfigurationTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<ILogger<MongoService>> _mockLogger;
    private readonly IConfiguration _configuration;

    public MongoConnectionConfigurationTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockLogger = new Mock<ILogger<MongoService>>();

        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    // Testing to connect to a MongoDB database with a valid connection string and database name.
    [Fact]
    public void InitializeMongoService_WithAppSettingsConfiguration_ConnectsSuccessfully()
    {
        // Arrange
        var connectionString = _configuration["MongoDB:ConnectionString"];
        var databaseName = _configuration["MongoDB:DatabaseName"];

        SetupConfiguration(connectionString, databaseName);

        // Act & Assert
        VerifyServiceCreatedSuccessfully();
    }

    // Testing to connect to a MongoDB database with an invalid connection string.
    [Theory]
    [InlineData(null, "testdb", "connectionString")]
    [InlineData("mongodb://localhost:27017", null, "databaseName")]
    [InlineData(null, null, "connectionString")] // eller "databaseName" beroende på vilken som kontrolleras först
    public void InitializeMongoService_WithInvalidConfiguration_ThrowsArgumentNullException(
        string? connectionString,
        string? databaseName,
        string expectedParamName)
    {
        // Arrange
        SetupConfiguration(connectionString, databaseName);

        // Act & Assert
        VerifyServiceThrowsArgumentNullException(expectedParamName);
    }

    // Testing to connect to a MongoDB database with an empty connection string.
    private void SetupConfiguration(string? connectionString, string? databaseName)
    {
        _mockConfiguration.Setup(c => c["MongoDB:ConnectionString"])
            .Returns(connectionString);
        _mockConfiguration.Setup(c => c["MongoDB:DatabaseName"])
            .Returns(databaseName);
    }


    // ** Helper methods.

    // Verify that the service was created successfully.
    private void VerifyServiceCreatedSuccessfully()
    {
        var exception = Record.Exception(() =>
            CreateMongoService());

        Assert.Null(exception);
    }

    // Verify that the service throws an ArgumentNullException with the expected parameter name.
    private void VerifyServiceThrowsArgumentNullException(string expectedParamName)
    {
        var exception = Assert.Throws<ArgumentNullException>(() =>
            CreateMongoService());

        Assert.Equal(expectedParamName, exception.ParamName);
    }

    // Create a new MongoService instance, Singleton pattern.
    private MongoService CreateMongoService()
    {
        return new MongoService(_mockConfiguration.Object, _mockLogger.Object);
    }
}