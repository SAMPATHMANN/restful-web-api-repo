using DbModels = DemomannWebApi.Profile.Data.Models;

namespace DemomannWebApi.Profile.Repos;
public interface IProfileRepo
{
    Task<List<DbModels.Profile>> GetAsync();

    Task<DbModels.Profile?> GetAsync(string id) ;

    Task CreateAsync(DbModels.Profile newBook);

    Task UpdateAsync(string id, DbModels.Profile updatedBook);

    Task RemoveAsync(string id);
}