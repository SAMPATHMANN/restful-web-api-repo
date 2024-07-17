
using DemomannWebApi.Profile.Repos;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ServiceModels = DemomannWebApi.Profile.Services.Models;

namespace DemomannWebApi.Profile.Services;

public class ProfileService : IProfileService
{
    private readonly IProfileRepo _profileRepo;
    public ProfileService(ILogger<ProfileService> logger, IProfileRepo profileRepo)
    {
        this._profileRepo = profileRepo;
    }
    public async Task<ServiceModels.ProfileSearch> GetProfilesAsync(ServiceModels.ProfileSearch request)
    {
        try
        {

            if (request != null && request.Authenticate
                && (string.IsNullOrEmpty(request.Email)))
                throw new Exception("Email is required");

            if (request != null && request.Authenticate
                            && (string.IsNullOrEmpty(request.Password)))
                throw new Exception("Password is required");

            var dbProfileListQry = _profileRepo.GetAsyncQuery(request);
            if (dbProfileListQry != null)
                request.TotalCount = await dbProfileListQry.CountAsync();

            if (request.TotalCount > 0)
            {
                request.Data = (await dbProfileListQry.Skip(request.Skip).Take(request.Take).ToListAsync())
                                .Select(x =>
                                {
                                    return new ServiceModels.Profile()
                                    {
                                        FirstName = x.FirstName,
                                        LastName = x.LastName,
                                        Email = x.Email
                                    };
                                }).ToList();

                request.IsValidUser = request.Authenticate && !string.IsNullOrEmpty(request.Email) && !string.IsNullOrEmpty(request.Password) ? true : false;
            }

            request.Password = null;

            return request;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task<ServiceModels.Profile> SaveProfileAsync(ServiceModels.Profile request)
    {
        try
        {
            PerformValidations(request);

            var dbProfiles = await _profileRepo.GetAsync(new ServiceModels.ProfileSearch { Email = request.Email });

            if (dbProfiles != null && dbProfiles.Any())
            {
                await _profileRepo.UpdateAsync(dbProfiles[0].Id, dbProfiles[0]);
            }
            else
            {
                var dbProfile = new Data.Models.Profile()
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Password = request.Password
                };
                await _profileRepo.CreateAsync(dbProfile);
                if (dbProfiles == null) dbProfiles = new List<Data.Models.Profile>();
                dbProfiles.Add(dbProfile);

            }

            var retVal = new ServiceModels.Profile()
            {
                FirstName = dbProfiles[0].FirstName,
                LastName = dbProfiles[0].LastName,
                Email = dbProfiles[0].Email,
                Password = dbProfiles[0].Password
            };
            return retVal;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private static void PerformValidations(ServiceModels.Profile request)
    {
        if (request == null)
            throw new Exception("Request Object Required");
        if (request != null && string.IsNullOrEmpty(request.Email))
            throw new Exception(" Email Required");
        if (request != null && string.IsNullOrEmpty(request.Password))
            throw new Exception(" Password Required");
        if (request != null
        && !string.IsNullOrEmpty(request.PasswordReEnter)
        && string.IsNullOrEmpty(request.FirstName))
            throw new Exception(" First Name Required");
        if (request != null
        && !string.IsNullOrEmpty(request.PasswordReEnter)
        && string.IsNullOrEmpty(request.LastName))
            throw new Exception(" Last Name Required");
        if (request != null
                && (!string.IsNullOrEmpty(request.LastName) || !string.IsNullOrEmpty(request.FirstName))
                && string.IsNullOrEmpty(request.PasswordReEnter))
            throw new Exception(" Re-Enter Password Required");

        if (request != null
    && !string.IsNullOrEmpty(request.PasswordReEnter)
    && !string.IsNullOrEmpty(request.Password)
    && request.Password != request.PasswordReEnter)
            throw new Exception(" Passwords don't match");
    }
}