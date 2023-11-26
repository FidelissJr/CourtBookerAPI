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
        public Agendamento? VerificaAgendamentoUsuarioExistente(DateTime dataInicial, string cpf)
        {
            return _repository.VerificaAgendamentoUsuarioExistente(dataInicial, cpf);
        }
        public dynamic ValidarAgendamento(Agendamento agendamento)
        {
            Quadra? quadra = _quadraService.BuscarQuadra(agendamento.IdQuadra);

            if (quadra == null)
                throw new BadHttpRequestException("Quadra indisponível");

            agendamento.DiasSemana ??= Array.Empty<int>();

            CustomHelper.IsValidEmail(agendamento.EmailUsuario);

            Agendamento agendamentoAux = VerificaAgendamentoUsuarioExistente(agendamento.DataInicio, agendamento.CpfUsuario);

            if (agendamentoAux != null)
                throw new BadHttpRequestException($"Não é possível agendar dois agendamento na mesma data com o mesmo CPF");

            if (!agendamento.Evento && agendamento.Recorrente)
                throw new BadHttpRequestException("Agendamentos não podem ser recorrentes. Torne-o um Evento");

            if (agendamento.Evento && agendamento.Recorrente)
            {
                if (agendamento.DataFim == null)
                    throw new BadHttpRequestException("Um evento deve conter uma data final");

                if (!agendamento.DiasSemana.Any())
                    throw new BadHttpRequestException("Eventos recorrentes devem ter Dias da Semana definidos!");

                List<Agendamento> agendamentosEvento = GerarEventosSemanais(agendamento);

                return _repository.AdicionarEvento(agendamentosEvento);
            }
            else
            {
                VerificaAntecedenciaMinimaDe24Horas(agendamento.DataInicio.Date, agendamento.HorarioInicial);
                ValidaHorarioAgendamento(agendamento);
                return _repository.AdicionarAgendamento(agendamento);
            }

        }
        public void GetEmailMessage(Agendamento agendamento, out string message, out string receiver, out string subject, bool cancelar)
        {
            Quadra quadra = _quadraService.BuscarQuadra(agendamento.IdQuadra);

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
        private static void VerificaAntecedenciaMinimaDe24Horas(DateTime data, TimeSpan intervalo)
        {
            DateTime dataModificada = data.Add(intervalo);
            DateTime agora = DateTime.Now.AddHours(24);

            // Retorna true se a data modificada for pelo menos 24 horas anterior à data e hora atuais
            if (dataModificada <= agora)
                throw new BadHttpRequestException("O Agendamento deve ser cadastrado com no mínimo 1 dia de antecedência");
        }
        private static void ValidaHorarioAgendamento(Agendamento agendamento)
        {
            if(agendamento.HorarioFinal < agendamento.HorarioInicial)
                throw new BadHttpRequestException("O horário final deve ser depois do horário inicial!");

            TimeSpan diferenca = (agendamento.HorarioInicial - agendamento.HorarioFinal).Duration();
            TimeSpan duasHoras = TimeSpan.FromHours(2);

            if (diferenca > duasHoras)
                throw new BadHttpRequestException("O agendamento não deve durar mais do que duas horas.");
        }
        public static List<Agendamento> GerarEventosSemanais(Agendamento agendamento)
        {
            var eventos = new List<Agendamento>();

            for (var dt = agendamento.DataInicio; dt <= agendamento.DataFim; dt = dt.AddDays(1))
            {
                if (agendamento.DiasSemana.Contains((int)dt.DayOfWeek))
                {
                    agendamento.DataInicio = dt;
                    eventos.Add(new Agendamento(
                      agendamento.HorarioInicial,
                      agendamento.HorarioFinal,
                      dt,
                      agendamento.DataFim.GetValueOrDefault(),
                      agendamento.CpfUsuario,
                      agendamento.IdQuadra,
                      agendamento.StatusAgendamento,
                      agendamento.EmailUsuario,
                      agendamento.Presenca,
                      agendamento.Evento,
                      agendamento.Recorrente,
                      agendamento.DiasSemana
                    ));
                }
            }

            return eventos;
        }
    }
}
