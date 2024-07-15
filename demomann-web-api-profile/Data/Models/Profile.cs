

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DemomannWebApi.Profile.Data.Models;

public class Profile{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string? Name { get; set; } = null;

    [BsonElement("email")]
    public string? Email {get; set;} = null;
    
    [BsonElement("password")]
    public string? Password  { get; set; }= null;
}