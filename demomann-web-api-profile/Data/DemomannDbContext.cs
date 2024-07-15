using MongoDB.Driver;
using MongoDbModel = DemomannWebApi.Profile.Data.Models;

namespace DemomannWebApi.Profile.Data;

internal class DemomannDbContext: DbContextBase
{
    public IMongoDatabase? _dbContext = null;

    public IMongoCollection<MongoDbModel.Profile> Profiles { get; set; } = null!;
}






