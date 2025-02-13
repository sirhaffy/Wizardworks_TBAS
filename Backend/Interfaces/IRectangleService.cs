using Backend.Models;

namespace Backend.Interfaces;

public interface IRectangleService
{
    Task<List<Rectangle>> GetAll();
    Task<Rectangle> Add(Rectangle rectangle);
    Task DeleteAll();
}