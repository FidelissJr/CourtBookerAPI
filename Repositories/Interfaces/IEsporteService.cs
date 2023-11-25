using CourtBooker.Model;

namespace CourtBooker.Repositories.Interfaces
{
    public interface IEsporteService
    {
        public List<Esporte> ListarEsportes();
        public Esporte AdicionarEsporte(Esporte esporte);
        public bool ExcluirEsporte(int id);
    }
}
