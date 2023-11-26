using CourtBooker.Enuns;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace CourtBooker.Model
{
    public class Agendamento
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "O agendamento não possui tempo inícial")]
        public TimeSpan HorarioInicial { get; set; }
        [Required(ErrorMessage = "O agendamento não possui tempo final")]
        public TimeSpan HorarioFinal { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        [StringLength(11, ErrorMessage = "Formato do Cpf inválido", MinimumLength = 11)]
        [Required(ErrorMessage = "O campo Cpf é obrigatório e não pode ser deixado em branco")]
        public string CpfUsuario { get; set; }
        [Required(ErrorMessage = "O campo Quadra é obrigatório e não pode ser deixado em branco")]
        public int IdQuadra { get; set; }
        public EnumStatusAgendamento StatusAgendamento { get; set; }
        public string StatusAgendamentoAux => StatusAgendamento.ToString().ToLower();
        [EmailAddress]
        public string? EmailUsuario { get; set; }
        public bool Presenca { get; set; }
        public bool Evento { get; set; }
        public bool Recorrente { get; set; }
        [AllowNull]
        public int[]? DiasSemana { get; set; }

        public Agendamento()
        {

        }

        public Agendamento(TimeSpan horarioInicial, TimeSpan horarioFinal, DateTime dataInicio, DateTime dataFim, string cpfUsuario, int idQUadra, EnumStatusAgendamento statusAgendamento, string? emailUsuario, bool presenca, bool evento, bool recorrente, int[] diasSemana)
        {
            HorarioInicial = horarioInicial;
            HorarioFinal = horarioFinal;
            DataInicio = dataInicio;
            DataFim = dataFim;
            CpfUsuario = cpfUsuario;
            IdQuadra = idQUadra;
            StatusAgendamento = statusAgendamento;
            EmailUsuario = emailUsuario;
            Presenca = presenca;
            Evento = evento;
            Recorrente = recorrente;
            DiasSemana = diasSemana;
        }
    }
}
