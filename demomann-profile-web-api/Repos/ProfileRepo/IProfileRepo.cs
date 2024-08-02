using DbModels = DemomannWebApi.Profile.Data.Models;
using ServiceModels = DemomannWebApi.Profile.Services.Models;

namespace DemomannWebApi.Profile.Repos;
public interface IProfileRepo
{
    Task<List<DbModels.Profile>> GetAsync();

    Task<DbModels.Profile?> GetAsync(string id) ;

    Task CreateAsync(DbModels.Profile newBook);

    Task UpdateAsync(string id, DbModels.Profile updatedBook);

    Task RemoveAsync(string id);

    Task<List<DbModels.Profile>> GetAsync(ServiceModels.ProfileSearch request);

    MongoDB.Driver.Linq.IMongoQueryable<DbModels.Profile> GetAsyncQuery(ServiceModels.ProfileSearch request);
}