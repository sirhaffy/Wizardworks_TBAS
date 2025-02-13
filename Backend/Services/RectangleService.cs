using Backend.Interfaces;
using Backend.Models;
using MongoDB.Driver;

namespace Backend.Services;

public class RectangleService : IRectangleService
{
    private readonly IMongoCollection<Rectangle> _rectangles;
    
    public RectangleService(IMongoService mongoService)
    {
        _rectangles = mongoService.GetCollection<Rectangle>("rectangles");
    }

    public async Task<List<Rectangle>> GetAll() =>
        await _rectangles.Find(_ => true).ToListAsync();
    
    // public async Task<List<Rectangle>> GetAll() =>
    //     await (await _rectangles.FindAsync(_ => true)).ToListAsync();

    public async Task<Rectangle> Add(Rectangle rectangle)
    {
        await _rectangles.InsertOneAsync(rectangle);
        return rectangle;
    }

    public async Task DeleteAll()
    {
        await _rectangles.DeleteManyAsync(_ => true);
    }
}