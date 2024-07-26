using Demomann.common.enums;
using ServiceModels = DemomannWebApi.Profile.Services.Models;

namespace DemomannWebApi.Profile.Services;
public interface IProfileService
{
    Task<List<ProfileActionType>> GetProfileActionTypesAsync();
    Task<ServiceModels.ProfileSearch> GetProfilesAsync(ServiceModels.ProfileSearch request);
    Task<ServiceModels.Profile> SaveProfileAsync(ProfileActionTypeEnum saveType, ServiceModels.Profile request);
}