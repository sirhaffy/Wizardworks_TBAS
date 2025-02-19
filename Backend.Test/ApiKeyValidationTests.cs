using System.Text.Encodings.Web;
using Backend.Controller;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace BackendTest;

public class ApiKeyValidationTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly ApiKeyAuthenticationHandler _handler;
    private readonly DefaultHttpContext _httpContext;
    private const string TestApiKey = "test-api-key";

    public ApiKeyValidationTests ()
    {
        // Setup configuration mock
        _mockConfiguration = new Mock<IConfiguration>();

        // Setup HTTP context with required services
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton(_mockConfiguration.Object);

        _httpContext = CreateHttpContext(services);
        _handler = CreateAuthenticationHandler();

        // Initialize the handler
        InitializeHandler().Wait();
    }

    [Theory]
    [InlineData(TestApiKey, true, "ApiUser", null)]
    [InlineData("wrong-api-key", false, null, "Invalid API Key")]
    public async Task AuthenticateAsync_ReturnsExpectedResult(
        string configuredApiKey,
        bool expectedSuccess,
        string? expectedUserName,
        string? expectedErrorMessage)
    {
        // Arrange
        _mockConfiguration.Setup(c => c["API_KEY"]).Returns(configuredApiKey);

        // Act
        var result = await InvokeHandleAuthenticateAsync();

        // Assert
        Assert.Equal(expectedSuccess, result.Succeeded);

        if (expectedSuccess)
        {
            Assert.NotNull(result.Principal);
            Assert.Equal(expectedUserName, result.Principal?.Identity?.Name);
        }
        else
        {
            Assert.NotNull(result.Failure);
            Assert.Equal(expectedErrorMessage, result.Failure?.Message);
        }
    }

    private DefaultHttpContext CreateHttpContext(ServiceCollection services)
    {
        var httpContext = new DefaultHttpContext
        {
            RequestServices = services.BuildServiceProvider()
        };
        httpContext.Request.Headers["X-API-Key"] = TestApiKey;
        return httpContext;
    }

    private ApiKeyAuthenticationHandler CreateAuthenticationHandler()
    {
        var mockOptions = new Mock<IOptionsMonitor<AuthenticationSchemeOptions>>();
        mockOptions.Setup(x => x.CurrentValue).Returns(new AuthenticationSchemeOptions());
        mockOptions.Setup(x => x.Get(It.IsAny<string>())).Returns(new AuthenticationSchemeOptions());

        var loggerFactory = new Mock<ILoggerFactory>();

        return new ApiKeyAuthenticationHandler(
            mockOptions.Object,
            loggerFactory.Object,
            UrlEncoder.Default,
            _mockConfiguration.Object);
    }


    private async Task InitializeHandler()
    {
        await _handler.InitializeAsync(new AuthenticationScheme(
                "ApiKey",
                "ApiKey",
                typeof(ApiKeyAuthenticationHandler)),
            _httpContext);
    }

    private async Task<AuthenticateResult> InvokeHandleAuthenticateAsync()
    {
        var method = typeof(ApiKeyAuthenticationHandler)
                         .GetMethod("HandleAuthenticateAsync",
                             System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                     ?? throw new InvalidOperationException("HandleAuthenticateAsync method not found");

        return await (Task<AuthenticateResult>)(method.Invoke(_handler, Array.Empty<object>())
                                                ?? throw new InvalidOperationException("Method invocation returned null"));
    }
}