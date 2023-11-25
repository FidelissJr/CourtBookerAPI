using CourtBooker.Enuns;
using CourtBooker.Helpers;
using CourtBooker.Model;
using CourtBooker.Services;

namespace CourtBooker.Repositories.Interfaces
{
    public interface IAgendamentoService
    {
        public Agendamento? BuscarAgendamento(int id);
        public List<Agendamento> ListarAgendamentos();
        public List<Agendamento> ListarAgendamentosBloco(int idBloco);
        public Agendamento? VerificaAgendamentoUsuarioExistente(DateTime dataInicial, string cpf);
        public Agendamento AdicionarAgendamento(Agendamento agendamento);
        public bool AdicionarEvento(Agendamento agendamento);
        public bool ExcluirAgendamento(int id);
    }
}
