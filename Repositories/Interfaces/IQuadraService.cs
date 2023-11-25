using CourtBooker.Model;

namespace CourtBooker.Repositories.Interfaces
{
    public interface IQuadraService
    {
        public List<Quadra> ListarQuadras();
        public Quadra? BuscarQuadra(int id);
        public Quadra AdicionarQuadra(Quadra quadra);
        public bool AdicionarQuadraEsporte(int idQuadra, int idEsporte);
        public bool ExcluirQuadraEsporte(int idQuadra, int idEsporte);
        public bool ExcluirQuadra(int id);
    }
}
