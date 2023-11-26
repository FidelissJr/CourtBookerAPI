using CourtBooker.Model;

namespace CourtBooker.Repositories.Interfaces
{
    public interface IAgendamentoService
    {
        public Agendamento? BuscarAgendamento(int id);
        public List<Agendamento> ListarAgendamentos();
        public List<Agendamento> ListarAgendamentosBloco(int idBloco);
        public Agendamento? VerificaAgendamentoUsuarioExistente(DateTime dataInicial, string cpf);
        public Agendamento AdicionarAgendamento(Agendamento agendamento);
        public List<Agendamento> AdicionarEvento(List<Agendamento> agendamentos);
        public bool ExcluirAgendamento(int id);
    }
}
