using ServiceModels = DemomannWebApi.Profile.Services.Models;

namespace DemomannWebApi.Profile.Services;
public interface IProfileService
{
    Task<ServiceModels.ProfileSearch> GetProfilesAsync(ServiceModels.ProfileSearch request);
    Task<ServiceModels.Profile> SaveProfileAsync(ServiceModels.Profile request);
}