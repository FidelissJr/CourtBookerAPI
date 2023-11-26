using CourtBooker.Model;
using CourtBooker.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CourtBooker.Repositories.Mongo
{
    public class MongoUsuarioRepository : MongoRepository, IUsuarioService
    {
        private readonly IMongoCollection<Usuario> _collection = database.GetCollection<Usuario>("usuario");
        public List<Usuario> ListarUsuarios()
        {
            List<Usuario> usuarios = _collection.Find(new BsonDocument()).ToList();
            return usuarios;
        }
        public Usuario BuscarUsuario(string cpf)
        {
            var filter = Builders<Usuario>.Filter.Eq("Cpf", cpf);
            var usuario = _collection.Find(filter).FirstOrDefault();
            return usuario;
        }
        public Usuario AdicionarUsuario(Usuario user)
        {
            var collection = database.GetCollection<Usuario>("usuario");
            collection.InsertOne(user);
            return user;
        }
        public bool EditarUsuario(Usuario user)
        {
            var filtro = Builders<Usuario>.Filter.Eq(u => u.Cpf, user.Cpf);
            var atualizacao = Builders<Usuario>.Update
                .Set(u => u.Nome, user.Nome)
                .Set(u => u.Senha, user.Senha)
                .Set(u => u.Email, user.Email)
                .Set(u => u.TipoUsuario, user.TipoUsuario)
                .Set(u => u.DataFimBolsa, user.DataFimBolsa);

            _collection.UpdateOne(filtro, atualizacao);

            return true;
        }
        public bool ExcluirUsuario(string cpf)
        {
            var deleteFilter = Builders<Usuario>.Filter.Eq(u => u.Cpf, cpf);
            _collection.DeleteOne(deleteFilter);
            return true;
        }
    }
}
