using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend.Models;

public class Rectangle
{
    public ObjectId Id { get; set; }
    public string Color { get; set; }
}
