using CourtBooker.Model;
using CourtBooker.Repositories.Interfaces;

namespace CourtBooker.Services
{
    public class EsporteService
    {
        private readonly IEsporteService _repository;

        public EsporteService(IEsporteService repository)
        {
            _repository = repository;
        }
        public List<Esporte> ListarEsportes()
        {
            return _repository.ListarEsportes();
        }

        public Esporte AdicionarEsporte(Esporte esporte)
        {
            return _repository.AdicionarEsporte(esporte);
        }

        public bool ExcluirEsporte(int id)
        {
            return _repository.ExcluirEsporte(id);
        }
    }
}
