using MongoDB.Bson.Serialization;

namespace Demomann.common.lib.Helpers;
public static class MongoDbHelper{
    public static void RegisterClassMap<T>(){
        BsonClassMap.RegisterClassMap<T>(map => {
            map.AutoMap();
            map.SetIgnoreExtraElements(true);
        });
    }
}

