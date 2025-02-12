using MongoDB.Driver;

namespace Backend.Interfaces;

public interface IMongoService
{
    IMongoCollection<T> GetCollection<T>(string collectionName);
    IMongoDatabase GetDatabase();
}