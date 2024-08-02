using DemomannWebApi.Profile.Data;
using DemomannWebApi.Profile.Services.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using DbModels = DemomannWebApi.Profile.Data.Models;
using ServiceModels = DemomannWebApi.Profile.Services.Models;

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

    public async Task<List<DbModels.Profile>> GetAsync( ProfileSearch request)
    {
        try
        {
            var profilesQry = GetAsyncQuery(request);

            var profiles = await profilesQry.ToListAsync();

            return profiles;
        }
        catch (Exception ex){
            throw ex;
        }
        
    }

    public MongoDB.Driver.Linq.IMongoQueryable<DbModels.Profile> GetAsyncQuery(ProfileSearch request)
    {
        var profilesQry = _profileCollection.AsQueryable();
        if (request != null)
        {
            if (!string.IsNullOrEmpty(request.Email))
            {
                profilesQry = (MongoDB.Driver.Linq.IMongoQueryable<DbModels.Profile>)profilesQry.Where((x => !string.IsNullOrEmpty(x.Email) && x.Email.ToLower() == request.Email.ToLower())).AsQueryable();
            }
            if (!string.IsNullOrEmpty(request.Password))
            {
                profilesQry = (MongoDB.Driver.Linq.IMongoQueryable<DbModels.Profile>)profilesQry.Where((x => !string.IsNullOrEmpty(x.Password) && x.Password == request.Password)).AsQueryable();
            }
            profilesQry = (MongoDB.Driver.Linq.IMongoQueryable<DbModels.Profile>)profilesQry.Skip(request.Skip).Take(request.Take);
        }

        return profilesQry;
    }
}