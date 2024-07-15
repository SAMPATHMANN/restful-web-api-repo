

using DemomannWebApi.Profile.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DemomannWebApi.Profile.Repos;

internal class BaseRepo: DemomannDbContext{
    internal BaseRepo(IOptions<DemomannDbContext> dbContextOptions){
        var mongoClient = new MongoClient(dbContextOptions.Value.ConnectionString);
        _dbContext = mongoClient.GetDatabase(dbContextOptions.Value.DatabaseName);
    }
}