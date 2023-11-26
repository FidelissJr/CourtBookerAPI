using CourtBooker.Model;
using CourtBooker.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CourtBooker.Repositories.Mongo
{
    public class MongoQuadraRepository : MongoRepository, IQuadraService
    {
        private readonly IMongoCollection<Quadra> _collection = database.GetCollection<Quadra>("quadra");
        private readonly IMongoCollection<BsonDocument> _relationCollection = database.GetCollection<BsonDocument>("quadra_esporte");
        public List<Quadra> ListarQuadras()
        {
            var lookup = new BsonDocument
            {
                {
                    "$lookup", new BsonDocument
                    {
                        { "from", "quadra_esporte" },
                        { "localField", "_id" },
                        { "foreignField", "idQuadra" },
                        { "as", "esportesRelacionados" }
                    }
                }
            };

            var project = new BsonDocument
            {
                {
                    "$project", new BsonDocument
                    {
                        { "Nome", 1 },
                        { "IdBloco", 1 },
                        { "esportesRelacionados", 1 }
                    }
                }
            };

            var unwind = new BsonDocument
            {
                {
                    "$unwind", new BsonDocument
                    {
                        { "path", "$esportesRelacionados" },
                        { "preserveNullAndEmptyArrays", true }
                    }
                }
            };

            var group = new BsonDocument
            {
                {
                    "$group", new BsonDocument
                    {
                        { "_id", "$_id" },
                        { "Nome", new BsonDocument("$first", "$Nome") },
                        { "IdBloco", new BsonDocument("$first", "$IdBloco") },
                        { "IdTiposEsporte", new BsonDocument("$push", "$esportesRelacionados.idEsporte") }
                    }
                }
            };

            var pipeline = new[] { lookup, project, unwind, group };
            var quadrasComEsportes = _collection.Aggregate<Quadra>(pipeline).ToList();

            foreach (var quadra in quadrasComEsportes)
            {
                Console.WriteLine(quadra);
            }

            return quadrasComEsportes;
        }
        public Quadra BuscarQuadra(int id)
        {
            var filter = Builders<Quadra>.Filter.Eq("Id", id);
            return _collection.Find(filter).FirstOrDefault();
        }
        public Quadra AdicionarQuadra(Quadra Quadra)
        {
            Quadra.Id = new Random().Next();
            _collection.InsertOne(Quadra);
            return Quadra;
        }
        public bool ExcluirQuadra(int id)
        {
            var deleteFilter = Builders<Quadra>.Filter.Eq(u => u.Id, id);
            _collection.DeleteOne(deleteFilter);
            return true;
        }
        public bool AdicionarQuadraEsporte(int idQuadra, int idEsporte)
        {
            object quadraEsporte = new { idQuadra, idEsporte };
            BsonDocument bsonDocument = quadraEsporte.ToBsonDocument();
            _relationCollection.InsertOne(bsonDocument);
            return true;
        }
        public bool ExcluirQuadraEsporte(int idQuadra, int idEsporte)
        {
            var filtro = Builders<BsonDocument>.Filter.Eq("idQuadra", idQuadra) & Builders<BsonDocument>.Filter.Eq("idEsporte", idEsporte);
            _relationCollection.DeleteOneAsync(filtro);
            return true;
        }
    }
}
