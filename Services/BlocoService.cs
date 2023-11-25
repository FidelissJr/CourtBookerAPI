using CourtBooker.Model;
using CourtBooker.Repositories.Interfaces;

namespace CourtBooker.Services
{
    public class BlocoService
    {
        private readonly IBlocoService _repository;

        public BlocoService(IBlocoService repository)
        {
            _repository = repository;
        }

        public List<Bloco> ListarBlocos()
        {
            return _repository.ListarBlocos();
        }

        public Bloco AdicionarBloco(Bloco bloco)
        {
            return _repository.AdicionarBloco(bloco);
        }

        public bool ExcluirBloco(int id)
        {
            return _repository.ExcluirBloco(id);
        }
    }
}
