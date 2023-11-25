using MongoDB.Bson;
using MongoDB.Driver;


namespace CourtBooker.Services
{
    public static class MongoRepository
    {
        static MongoClient dbClient = new("mongodb://127.0.0.1:27017/?directConnection=true&serverSelectionTimeoutMS=2000&appName=mongosh+2.1.0");
        static dynamic database = dbClient.GetDatabase("courtbooker");


        async public static void Teste()
        {
            var dbList = dbClient.ListDatabases().ToList();

            
            var collection = database.GetCollection<BsonDocument>("usuario");

            var document = new BsonDocument { { "student_id", 10000 }, {
                "scores",
                new BsonArray {
                new BsonDocument { { "type", "exam" }, { "score", 88.12334193287023 } },
                new BsonDocument { { "type", "quiz" }, { "score", 74.92381029342834 } },
                new BsonDocument { { "type", "homework" }, { "score", 89.97929384290324 } },
                new BsonDocument { { "type", "homework" }, { "score", 82.12931030513218 } }
                }
                }, { "class_id", 480 }
            };

            await collection.InsertOneAsync(document);
        }
    }
}
