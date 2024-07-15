using System.ComponentModel.DataAnnotations;
using DemomannWebApi.Profile.Repos;
using ServiceModels = DemomannWebApi.Profile.Services.Models;

namespace DemomannWebApi.Profile.Services;

public class ProfileService : IProfileService
{
    private readonly IProfileRepo _profileRepo;
    public ProfileService(ILogger<ProfileService> logger, IProfileRepo profileRepo) {
        this._profileRepo = profileRepo;
    }
    public async Task<List<ServiceModels.Profile>> GetProfilesAsync()
    {
        try {
            var dbProfileList = await _profileRepo.GetAsync();
            var retVal = dbProfileList
            .Select(x=> {
                return new ServiceModels.Profile(){
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email
                    };
                }).ToList();

            return retVal;
        }
        catch (Exception ex){
            throw ex;
        }
    }

    public async Task<ServiceModels.Profile> SaveProfileAsync(ServiceModels.Profile request)
    {
        try {
            var dbProfile = new Data.Models.Profile(){
                  Id = request.Id,
                  Name = request.Name,
                  Email = request.Email,
                  Password = request.Password
            };

            if(string.IsNullOrEmpty(dbProfile.Id))
            await _profileRepo.CreateAsync(dbProfile);
            else
            await _profileRepo.UpdateAsync(dbProfile.Id, dbProfile);

            dbProfile = await _profileRepo.GetAsync(dbProfile.Id);
            
            var retVal = new ServiceModels.Profile(){
                Id = dbProfile.Id,
                Name = dbProfile.Name,
                Email = dbProfile.Email,
                Password = dbProfile.Password
            };
            return retVal;
        }
        catch (Exception ex){
            throw ex;
        }
    }
}