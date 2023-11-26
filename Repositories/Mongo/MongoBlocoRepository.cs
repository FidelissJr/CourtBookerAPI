using CourtBooker.Model;
using CourtBooker.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CourtBooker.Repositories.Mongo
{
    public class MongoBlocoRepository : MongoRepository, IBlocoService
    {
        private readonly IMongoCollection<Bloco> _collection = database.GetCollection<Bloco>("bloco");
        public List<Bloco> ListarBlocos()
        {
            return _collection.Find(new BsonDocument()).ToList();
        }

        public Bloco AdicionarBloco(Bloco bloco)
        {
            bloco.Id = new Random().Next();
            _collection.InsertOne(bloco);
            return bloco;
        }
 
        public bool ExcluirBloco(int id)
        {
            var deleteFilter = Builders<Bloco>.Filter.Eq(u => u.Id, id);
            _collection.DeleteOne(deleteFilter);
            return true;
        }
    }
}
