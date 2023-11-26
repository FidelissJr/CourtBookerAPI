using MongoDB.Bson;
using MongoDB.Driver;


namespace CourtBooker.Repositories.Mongo
{
    public class MongoRepository
    {
        private static MongoClient dbClient = new("mongodb://127.0.0.1:27017/?directConnection=true&serverSelectionTimeoutMS=2000&appName=mongosh+2.1.0");
        protected static readonly IMongoDatabase database = dbClient.GetDatabase("courtbooker");
    }
}

