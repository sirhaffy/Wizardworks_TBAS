using MongoDB.Driver;

namespace Backend.Services;

public class MongoService : IMongoService
{
    private readonly IMongoDatabase _database;
    private readonly ILogger<MongoService> _logger;

    public MongoService(IConfiguration config, ILogger<MongoService> logger)
    {
        _logger = logger;
        try
        {
            var connectionString = config["MongoDB:ConnectionString"];
            var databaseName = config["MongoDB:DatabaseName"];
            _logger.LogInformation($"Connecting to database: {databaseName}");
            
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }
        catch (Exception ex)
        {
            _logger.LogError($"MongoDB connection error: {ex.Message}");
            throw;
        }
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        _logger.LogInformation($"Getting collection: {collectionName}");
        return _database.GetCollection<T>(collectionName);
    }

    public IMongoDatabase GetDatabase() => _database;
}
