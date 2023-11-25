using CourtBooker.Model;
using CourtBooker.Repositories.Interfaces;
using CourtBooker.Repositories.Postgres;

namespace CourtBooker.Services
{
    public class QuadraService : PostgresRepository
    {
        private readonly IQuadraService _repository;
        public QuadraService(IQuadraService repository)
        {
            _repository = repository;
        }

        public List<Quadra> ListarQuadras()
        {
            return _repository.ListarQuadras();
        }

        public Quadra? BuscarQuadra(int id)
        {
            return _repository.BuscarQuadra(id);
        }

        public Quadra AdicionarQuadra(Quadra quadra)
        {
            return _repository.AdicionarQuadra(quadra);
        }

        public bool AdicionarQuadraEsporte(int idQuadra, int idEsporte)
        {
            return _repository.AdicionarQuadraEsporte(idQuadra, idEsporte);
        }

        public bool ExcluirQuadraEsporte(int idQuadra, int idEsporte)
        {
            return _repository.ExcluirQuadraEsporte(idQuadra, idEsporte);
        }

        public bool ExcluirQuadra(int id)
        {
            return _repository.ExcluirQuadra(id);
        }
    }
}
