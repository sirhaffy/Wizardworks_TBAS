using MongoDB.Driver;

namespace Backend.Services;

public interface IMongoService
{
    IMongoCollection<T> GetCollection<T>(string collectionName);
    IMongoDatabase GetDatabase();
}