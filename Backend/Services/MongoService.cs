using Backend.Interfaces;
using MongoDB.Driver;

namespace Backend.Services;

    public class MongoService : IMongoService
{
    private readonly IMongoDatabase _database;
    private readonly ILogger<MongoService> _logger;
    private readonly MongoClient _client;

    public MongoService(IConfiguration config, ILogger<MongoService> logger)
    {
        _logger = logger;
        try
        {
            // Hämta host från miljövariabel
            var host = Environment.GetEnvironmentVariable("MONGODB_HOST") ?? "localhost";
            _logger.LogInformation("Using MongoDB host: {Host}", host);

            // Hämta användarnamn och lösenord från config eller miljövariabler
            var user = Environment.GetEnvironmentVariable("MONGODB_USER") ?? "appuser";
            var password = Environment.GetEnvironmentVariable("MONGODB_PASSWORD") ?? "apppassword";
            var databaseName = config.GetValue<string>("MongoDB:DatabaseName") ?? "tbas_db";

            var connectionString = $"mongodb://{user}:{password}@{host}:27017/{databaseName}?authSource=admin";

            _logger.LogInformation("Attempting to connect to MongoDB...");
            _logger.LogInformation("Using database: {DatabaseName}", databaseName);

            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.ServerSelectionTimeout = TimeSpan.FromSeconds(15); // Öka timeout
            _client = new MongoClient(settings);
            _database = _client.GetDatabase(databaseName);

            // Testa anslutningen
            var pingCommand = new MongoDB.Bson.BsonDocument("ping", 1);
            var pingResult = _database.RunCommand<MongoDB.Bson.BsonDocument>(pingCommand);

            _logger.LogInformation("Successfully connected to MongoDB and verified connection");
        }
        catch (MongoConnectionException ex)
        {
            _logger.LogError(ex, "Failed to connect to MongoDB. Connection error");
            throw new Exception("Could not connect to MongoDB. Please check if the database is running and accessible.", ex);
        }
        catch (TimeoutException ex)
        {
            _logger.LogError(ex, "Connection to MongoDB timed out");
            throw new Exception("Connection to MongoDB timed out. Please check network connectivity and database availability.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while connecting to MongoDB");
            throw new Exception("An unexpected error occurred while connecting to MongoDB.", ex);
        }
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        if (string.IsNullOrEmpty(collectionName))
            throw new ArgumentNullException(nameof(collectionName));

        try
        {
            _logger.LogInformation("Getting collection: {CollectionName}", collectionName);
            return _database.GetCollection<T>(collectionName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting collection: {CollectionName}", collectionName);
            throw new Exception($"Failed to get collection: {collectionName}", ex);
        }
    }

    public IMongoDatabase GetDatabase()
    {
        if (_database == null)
            throw new InvalidOperationException("Database connection not initialized");
        return _database;
    }
}