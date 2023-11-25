using CourtBooker.Model;

namespace CourtBooker.Repositories.Interfaces
{
    public interface IBlocoService
    {
        public List<Bloco> ListarBlocos();
        public Bloco AdicionarBloco(Bloco bloco);
        public bool ExcluirBloco(int id);
    }
}
