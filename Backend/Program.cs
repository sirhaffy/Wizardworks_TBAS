using Backend.Controller;
using Backend.Services;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

// CORS
var allowedOrigins = builder.Configuration["AllowedOrigins"]; // Läser värdet från appsettings.json

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(allowedOrigins ?? throw new InvalidOperationException())
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});


// Authentication
builder.Services.AddAuthentication("ApiKey")
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKey", null);

builder.Services.AddSingleton<IMongoService, MongoService>();
builder.Services.AddScoped<IRectangleService, RectangleService>();

var app = builder.Build();

app.UseRouting();
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();