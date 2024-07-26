
using Demomann.common.enums;
using DemomannWebApi.Profile.Repos;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ServiceModels = DemomannWebApi.Profile.Services.Models;
using DataModels = DemomannWebApi.Profile.Data.Models;
using Demomann.common.Helpers;
using Microsoft.AspNetCore.Mvc.Diagnostics;

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
                                        Email = x.Email,
                                        Phone = x.Phone,
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

    public async Task<ServiceModels.Profile> SaveProfileAsync(ProfileActionTypeEnum saveType, ServiceModels.Profile request)
    {
        try
        {
            if (request == null)
                throw new Exception("Request Required");
            if (string.IsNullOrEmpty(request.Email))
                throw new Exception(" Email Required");

            var dbProfiles = await _profileRepo.GetAsync(new ServiceModels.ProfileSearch { Email = request.Email });

            if(saveType == ProfileActionTypeEnum.ChangeEmail && !string.IsNullOrEmpty(request.EmailReenter)){

                var changeEmailProfiles = await _profileRepo.GetAsync(new ServiceModels.ProfileSearch { Email = request.EmailReenter }); 
                if(changeEmailProfiles != null && changeEmailProfiles.Any())
                    dbProfiles.AddRange(changeEmailProfiles);
            }

            var anyDbProfiles = dbProfiles != null && dbProfiles.Any();

            PerformValidations(saveType,
            request,
            anyDbProfiles ? dbProfiles : null);

            if (dbProfiles != null && dbProfiles.Any())
            {
                switch (saveType)
                {
                    case ProfileActionTypeEnum.UpdateProfile:
                        dbProfiles[0].FirstName = request.FirstName;
                        dbProfiles[0].LastName = request.LastName;
                        dbProfiles[0].Phone = request.Phone;
                        break;
                    case ProfileActionTypeEnum.ChangeEmail:
                        dbProfiles[0].Email = request.Email;
                        break;
                    case ProfileActionTypeEnum.ResetPassword:
                        dbProfiles[0].Password = request.Password;
                        break;
                }


                await _profileRepo.UpdateAsync(dbProfiles[0].Id, dbProfiles[0]);
            }
            else
            {
                var dbProfile = new Data.Models.Profile()
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Password = request.Password,
                    Phone = request.Phone,
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
                Phone = dbProfiles[0].Phone,
            };
            return retVal;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private static void PerformValidations(ProfileActionTypeEnum saveType,
    ServiceModels.Profile request,
    List<DataModels.Profile>? dbProfiles = null)

    {
        switch (saveType)
        {
            case ProfileActionTypeEnum.Register:
                if (string.IsNullOrEmpty(request.FirstName))
                    throw new Exception("First Name Required.");
                if (string.IsNullOrEmpty(request.LastName))
                    throw new Exception("Last Name Required.");
                if (string.IsNullOrEmpty(request.Phone))
                    throw new Exception("Phone# Required.");
                if (string.IsNullOrEmpty(request.Password))
                    throw new Exception("Password Required");
                if (string.IsNullOrEmpty(request.PasswordReEnter))
                    throw new Exception("Re Enter Password Required.");
                if (request.Password != request.PasswordReEnter)
                    throw new Exception("Passwords need to match.");
                if (dbProfiles!= null)
                    throw new Exception("Profile already existed with same email.");
                break;
            case ProfileActionTypeEnum.ResetPassword:
                if (string.IsNullOrEmpty(request.Password))
                    throw new Exception("Password Required");
                if (string.IsNullOrEmpty(request.PasswordReEnter))
                    throw new Exception("Re Enter Password Required.");
                if (request.Password != request.PasswordReEnter)
                    throw new Exception("Passwords need to match.");
                if (dbProfiles == null)
                    throw new Exception("No profile with email.");
                break;
            case ProfileActionTypeEnum.UpdateProfile:
                if (dbProfiles == null)
                    throw new Exception("No profile with email.");
                if (string.IsNullOrEmpty(request.FirstName))
                    throw new Exception("First Name Required.");
                if (string.IsNullOrEmpty(request.LastName))
                    throw new Exception("Last Name Required.");
                if (string.IsNullOrEmpty(request.Phone))
                    throw new Exception("Phone Required.");
                    break;
            case ProfileActionTypeEnum.ChangeEmail:
                if (dbProfiles == null)
                    throw new Exception("No profile with email.");
                if(dbProfiles != null && dbProfiles.Any(x=> x.Email?.ToLower() == request?.EmailReenter.ToLower()))
                    throw new Exception("Email Already existed.");
                break;
        }

    }

    public Task<List<ProfileActionType>> GetProfileActionTypesAsync()
    {
        List<ProfileActionType> enumValues = GetEnumValues();
        var retVal = Task.Run(() =>
        {
            return enumValues;
        });
        return retVal;
    }

    private static List<ProfileActionType> GetEnumValues()
    {
        return ((ProfileActionTypeEnum[])Enum.GetValues(typeof(ProfileActionTypeEnum)))
            .Select(c => new ProfileActionType()
            {
                ActionId = (int)c,
                ActionName = c.ToString(),
                ActionDescription = c.GetDescription()
            }).ToList();
    }
}