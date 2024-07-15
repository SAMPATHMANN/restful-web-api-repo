using DemomannWebApi.Profile.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using DbModels = DemomannWebApi.Profile.Data.Models;

namespace DemomannWebApi.Profile.Repos;

internal class ProfileRepo : BaseRepo, IProfileRepo
{

    private readonly IMongoCollection<DbModels.Profile> _profileCollection;

    public ProfileRepo(IOptions<DemomannDbContext> dbContextOptions) : base(dbContextOptions)
    {
        _profileCollection = _dbContext.GetCollection<DbModels.Profile>("profiles");
    }

    public async Task<List<DbModels.Profile>> GetAsync() =>
        await _profileCollection.Find(_ => true).ToListAsync();

    public async Task<DbModels.Profile?> GetAsync(string id) =>
        await _profileCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(DbModels.Profile newprofile) =>
        await _profileCollection.InsertOneAsync(newprofile);

    public async Task UpdateAsync(string id, DbModels.Profile updatedProfile) =>
        await _profileCollection.ReplaceOneAsync(x => x.Id == id, updatedProfile);

    public async Task RemoveAsync(string id) =>
        await _profileCollection.DeleteOneAsync(x => x.Id == id);
}