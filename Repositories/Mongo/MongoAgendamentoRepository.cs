using CourtBooker.Model;
using CourtBooker.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CourtBooker.Repositories.Mongo
{
    public class MongoAgendamentoRepository : MongoRepository, IAgendamentoService
    {
        private readonly IMongoCollection<Agendamento> _collection = database.GetCollection<Agendamento>("agendamento");
        public Agendamento? BuscarAgendamento(int id)
        {
            var filter = Builders<Agendamento>.Filter.Eq(c => c.Id, id);
            return _collection.Find(filter).FirstOrDefault();
        }
        public List<Agendamento> ListarAgendamentos()
        {
            return _collection.Find(new BsonDocument()).ToList();
        }
        public List<Agendamento> ListarAgendamentosBloco(int idBloco)
        {
            var pipeline = new BsonDocument[]
            {
                new BsonDocument("$lookup", new BsonDocument
                    {
                    { "from", "quadra" },
                    { "localField", "idQuadra" },
                    { "foreignField", "id" },
                    { "as", "quadra" }
                }),

                new BsonDocument("$unwind", "$quadra"),

                new BsonDocument("$lookup", new BsonDocument
                    {
                        { "from", "bloco" },
                        { "localField", "quadra.idBloco" },
                        { "foreignField", "id" },
                        { "as", "bloco" }
                }),

                new BsonDocument("$unwind", "$bloco"),
                new BsonDocument("$match", new BsonDocument("quadra.IdBloco", idBloco)),

                new BsonDocument("$group", new BsonDocument
                {
                    { "_id", "$_id" },
                    { "HorarioInicial", new BsonDocument("$first", "$HorarioInicial") },
                    { "HorarioFinal", new BsonDocument("$first", "$HorarioFinal") },
                    { "DataInicio", new BsonDocument("$first", "$DataInicio") },
                    { "DataFim", new BsonDocument("$first", "$DataFim") },
                    { "CpfUsuario", new BsonDocument("$first", "$CpfUsuario") },
                    { "IdQuadra", new BsonDocument("$first", "$IdQuadra") },
                    { "StatusAgendamento", new BsonDocument("$first", "$StatusAgendamento") },
                    { "EmailUsuario", new BsonDocument("$first", "$EmailUsuario") },
                    { "Presenca", new BsonDocument("$first", "$Presenca") },
                    { "Evento", new BsonDocument("$first", "$Evento") },
                    { "Recorrente", new BsonDocument("$first", "$Recorrente") },
                    { "DiasSemana", new BsonDocument("$first", "$DiasSemana") },
                })
            };

            return _collection.Aggregate<Agendamento>(pipeline).ToList();

        }
        public Agendamento? VerificaAgendamentoUsuarioExistente(DateTime dataInicial, string cpf)
        {
            var filterBuilder = Builders<Agendamento>.Filter;
            var filter = filterBuilder.Eq(c => c.CpfUsuario, cpf) & filterBuilder.Eq(c => c.DataInicio, dataInicial);
            return _collection.Find(filter).FirstOrDefault();
        }
        public Agendamento AdicionarAgendamento(Agendamento agendamento)
        {
            ListarAgendamentosQuadraDia(agendamento.IdQuadra, agendamento);

            agendamento.Id = new Random().Next();
            _collection.InsertOne(agendamento);
            return agendamento;
        }
        public List<Agendamento> AdicionarEvento(List<Agendamento> agendamentos)
        {
            foreach (Agendamento a in agendamentos)
                AdicionarAgendamento(a);

            return agendamentos;
        }
        public bool ExcluirAgendamento(int id)
        {
            var deleteFilter = Builders<Agendamento>.Filter.Eq(u => u.Id, id);
            _collection.DeleteOne(deleteFilter);
            return true;
        }

        public void ListarAgendamentosQuadraDia(int idQuadra, Agendamento agendamento)
        {
            var builder = Builders<Agendamento>.Filter;
            var filtro = builder.Eq(agendamento => agendamento.IdQuadra, idQuadra) &
                         builder.Eq(agendamento => agendamento.DataInicio, agendamento.DataInicio);
                

            List<Agendamento> agendamentos = _collection.Find(filtro).ToList();

            foreach (var a in agendamentos)
            {
                if (a.HorarioInicial < agendamento.HorarioFinal && a.HorarioFinal > agendamento.HorarioInicial)
                {
                    throw new BadHttpRequestException("Choque de Horário!");
                }
                // Faça algo com cada agendamento
                Console.WriteLine(agendamento.Id);
            }
        }
    }
}
