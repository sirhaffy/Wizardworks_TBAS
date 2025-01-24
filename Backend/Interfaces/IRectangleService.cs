using Backend.Models;

namespace Backend.Services;

public interface IRectangleService
{
    Task<List<Rectangle>> GetAll();
    Task<Rectangle> Add(Rectangle rectangle);
    Task DeleteAll();
}