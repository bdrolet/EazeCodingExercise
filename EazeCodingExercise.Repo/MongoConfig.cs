using MongoDB.Driver;

namespace EazeCodingExercise.Repo
{
    public static class MongoConfig
    {
        public static MongoClient MongoClient;
        public static IMongoCollection<WebScrap> WebScrapCollection;

        static MongoConfig()
        {
            MongoClient = new MongoClient();
            var db = MongoClient.GetDatabase("EazeCodingExercise");
            WebScrapCollection = db.GetCollection<WebScrap>("WebScrap");
        }
    }
}
