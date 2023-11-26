using CourtBooker.Model;
using CourtBooker.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CourtBooker.Repositories.Mongo
{
    public class MongoEsporteRepository : MongoRepository, IEsporteService
    {
        private readonly IMongoCollection<Esporte> _collection = database.GetCollection<Esporte>("esporte");
        public List<Esporte> ListarEsportes()
        {
            return _collection.Find(new BsonDocument()).ToList();
        }
        public Esporte AdicionarEsporte(Esporte Esporte)
        {
            Esporte.Id = new Random().Next();
            _collection.InsertOne(Esporte);
            return Esporte;
        }
        public bool ExcluirEsporte(int id)
        {
            var deleteFilter = Builders<Esporte>.Filter.Eq(u => u.Id, id);
            _collection.DeleteOne(deleteFilter);
            return true;
        }
    }
}
