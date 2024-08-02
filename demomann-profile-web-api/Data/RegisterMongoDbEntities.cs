using Demomann.common.Helpers;
using DbModels = DemomannWebApi.Profile.Data.Models;

namespace DemomannWebApi.Profile.Data.Extensions;

public static class RegisterMongoDbEntities{
    public static void Register( ){
        //Register Mongo Db Entities
        MongoDbHelper.RegisterClassMap<DbModels.Profile>();

    }
} 