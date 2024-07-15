using ServiceModels = DemomannWebApi.Profile.Services.Models;

namespace DemomannWebApi.Profile.Services;
public interface IProfileService
{
    Task<List<ServiceModels.Profile>> GetProfilesAsync();
    Task<ServiceModels.Profile> SaveProfileAsync(ServiceModels.Profile request);
}