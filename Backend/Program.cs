using Backend.Controller;
using Backend.Interfaces;
using Backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using DotNetEnv;

Env.Load("../.env");

var builder = WebApplication.CreateBuilder(args);

// Service Configuration
ConfigureServices(builder.Services, builder.Configuration, builder.Environment, builder.Logging);

var app = builder.Build();

// Middleware Configuration
ConfigureMiddleware(app);

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment, ILoggingBuilder logging)
{
    // Basic Services
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddLogging();

    // Swagger Configuration
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rectangle API", Version = "v1" });
        c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
        {
            Name = "X-API-Key",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Description = "API Key authentication"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    // CORS Configuration
    services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigins", policy =>
        {
            var appIp = Environment.GetEnvironmentVariable("APP_IP");

            var origins = environment.EnvironmentName.ToLower() == "production"
                ? new[] { $"http://{appIp}" }
                : new[] {
                    "http://localhost:5173",  // Vite default
                    "http://localhost:3000",  // React default
                    "http://localhost",
                    "http://localhost:80"
                };

            // Validera att vi har giltiga origins i production
            if (environment.EnvironmentName.ToLower() == "production" && string.IsNullOrEmpty(appIp))
            {
                throw new InvalidOperationException("APP_IP environment variable is required in production");
            }

            policy.WithOrigins(origins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();

            // Använd logging direkt från ILoggingBuilder istället för app.Logger
            var logger = logging.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
            logger.LogInformation("CORS configured with origins: {@Origins}", origins);
        });
    });

    // Authentication
    services.AddAuthentication("ApiKey")
        .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", null);

    // Application Services
    services.AddSingleton<IMongoService, MongoService>();
    services.AddScoped<IRectangleService, RectangleService>();
}

void ConfigureMiddleware(WebApplication app)
{
    // Development Tools
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rectangle API V1");
        });
    }

    // Logging Middleware
    app.Use(async (context, next) =>
    {
        LogRequestDetails(app, context);
        await next();
        LogResponseDetails(app, context);
    });

    // Core Middleware Pipeline
    app.UseRouting();
    app.UseCors("AllowSpecificOrigins");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    // Startup Logging
    LogStartupInformation(app);
}

void LogRequestDetails(WebApplication app, HttpContext context)
{
    var corsHeaders = context.Response.Headers
        .Where(h => h.Key.StartsWith("Access-Control-"))
        .ToDictionary(h => h.Key, h => string.Join(", ", h.Value!));

    app.Logger.LogInformation("Request from origin: {Origin}",
        context.Request.Headers["Origin"].ToString());
    app.Logger.LogInformation("CORS headers: {@Headers}", corsHeaders);
}

void LogResponseDetails(WebApplication app, HttpContext context)
{
    app.Logger.LogInformation("CORS headers: {Headers}",
        context.Response.Headers["Access-Control-Allow-Origin"]!);
}

void LogStartupInformation(WebApplication app)
{
    app.Logger.LogInformation("Starting application...");
    app.Logger.LogInformation("Raw AllowedOrigins from config: {RawOrigins}",
        app.Configuration["AllowedOrigins"]);
    app.Logger.LogInformation("Raw AllowedOrigins from appsettings: {RawOrigins}",
        app.Configuration["AllowedOrigins"]);
}