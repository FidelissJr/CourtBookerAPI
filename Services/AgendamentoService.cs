using CourtBooker.Enuns;
using CourtBooker.Helpers;
using CourtBooker.Model;
using CourtBooker.Repositories.Interfaces;
using CourtBooker.Repositories.Postgres;
using Dapper;

namespace CourtBooker.Services
{
    public class AgendamentoService : PostgresRepository
    {
        private readonly IAgendamentoService _repository;
        private readonly IQuadraService _quadraService;

        public AgendamentoService(IAgendamentoService repository, IQuadraService quadraService)
        {
            _repository = repository;
            _quadraService = quadraService;
        }

        public List<Agendamento> ListarAgendamentos()
        {
            return _repository.ListarAgendamentos();
        }

        public List<Agendamento> ListarAgendamentosBloco(int idBloco)
        {
            return _repository.ListarAgendamentosBloco(idBloco);
        }

        public bool ExcluirAgendamento(int id)
        {
            return _repository.ExcluirAgendamento(id);
        }

        public Agendamento? BuscarAgendamento(int id)
        {
            return _repository.BuscarAgendamento(id);
        }

        public Agendamento? VerificaAgendamentoUsuarioExistente (DateTime dataInicial, string cpf)
        {
            return _repository.VerificaAgendamentoUsuarioExistente(dataInicial, cpf);
        }

        public dynamic ValidarAgendamento(Agendamento agendamento)
        {
            Quadra? quadra = _quadraService.BuscarQuadra(agendamento.IdQUadra);

            if (quadra == null)
                throw new BadHttpRequestException("Quadra indisponível");

            if (agendamento.DiasSemana == null)
                agendamento.DiasSemana = Array.Empty<int>();

            CustomHelper.IsValidEmail(agendamento.EmailUsuario);

            Agendamento agendamentoAux = VerificaAgendamentoUsuarioExistente(agendamento.DataInicio, agendamento.CpfUsuario);

            if (agendamentoAux != null)
                throw new BadHttpRequestException($"Não é possível agendar dois agendamento na mesma data com o mesmo CPF");

            if (agendamento.Evento && agendamento.Recorrente)
            {
                if (agendamento.DataFim == null)
                    throw new BadHttpRequestException("Um evento deve conter uma data final");

                if (!agendamento.DiasSemana.Any())
                    throw new BadHttpRequestException("Eventos recorrentes devem ter Dias da Semana definidos!");

                return _repository.AdicionarEvento(agendamento);
            }
            else
                return _repository.AdicionarAgendamento(agendamento);

        }

        public void GetEmailMessage(Agendamento agendamento, out string message, out string receiver, out string subject, bool cancelar)
        {
            Quadra quadra = _quadraService.BuscarQuadra(agendamento.IdQUadra);

            receiver = agendamento.EmailUsuario;
            subject = cancelar ? "Cancelamento Agendamento" : "Confirmação Agendamento";
            message = cancelar ? "Agendamento Cancelado\n\n" : "Agendamento realizado com sucesso\n\n";
            message += $"Data Agendamento: {agendamento.DataInicio.ToString("dd/MM/yyyy")}\n";
            message += $"Horário: {agendamento.HorarioInicial} - {agendamento.HorarioFinal}\n";
            message += $"Quadra: {quadra.Nome}";
        }
        public List<EnumValueDescription> ListarDiasSemana()
        {
            List<EnumValueDescription> result = EnumHelper.GetEnumValueDescriptionList<DiasSemana>();
            return result;
        }
    }
}
