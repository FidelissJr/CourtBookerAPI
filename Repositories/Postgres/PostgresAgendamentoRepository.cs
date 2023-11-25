using CourtBooker.Enuns;
using CourtBooker.Helpers;
using CourtBooker.Model;
using CourtBooker.Repositories.Interfaces;
using Dapper;

namespace CourtBooker.Repositories.Postgres
{
    public class PostgresAgendamentoRepository : PostgresRepository, IAgendamentoService
    {
        public Agendamento AdicionarAgendamento(Agendamento agendamento)
        {
            return WithConnection(dbConn =>
            {
                string sql = GetQuery(agendamento);
                return dbConn.QuerySingle<Agendamento>(sql, agendamento);
            });
        }

        public List<Agendamento> ListarAgendamentos()
        {
            return WithConnection(dbConn =>
            {
                string sql = QuerySelectAllAgendamento();
                return dbConn.Query<Agendamento>(sql).ToList();
            });
        }

        public List<Agendamento> ListarAgendamentosBloco(int idBloco)
        {
            return WithConnection(dbConn =>
            {
                string sql = QuerySelectAllAgendamento();
                sql += " join quadra q on id_quadra = q.id join bloco b on b.id = q.id_bloco where b.id = @IdBloco";
                return dbConn.Query<Agendamento>(sql, new { IdBloco = idBloco }).ToList();
            });

        }

        public bool ExcluirAgendamento(int id)
        {
            return WithConnection(dbConn =>
            {
                string sql = "Delete from agendamento WHERE id = @Id";
                int rowsAffected = dbConn.Execute(sql, new { Id = id });
                return rowsAffected > 0;
            });
        }

        public bool AdicionarEvento(Agendamento agendamento)
        {
            return WithConnection(dbConn =>
            {
                string sql = GetQuery(agendamento);
                int rowsAffected = dbConn.Execute(sql);
                return rowsAffected > 0;
            });
        }

        public Agendamento? BuscarAgendamento(int id)
        {
            return WithConnection(dbConn =>
            {
                string sql = QuerySelectAllAgendamento();
                sql += " WHERE id = @Id";
                return dbConn.QueryFirstOrDefault<Agendamento>(sql, new { Id = id });
            });
        }

        public Agendamento? VerificaAgendamentoUsuarioExistente(DateTime dataInicial, string cpf)
        {
            return WithConnection(dbConn =>
            {
                string sql = "SELECT 1 FROM agendamento";
                sql += " WHERE cpf_usuario = @Cpf AND dataInicial = @DataInicial";
                return dbConn.QueryFirstOrDefault<Agendamento>(sql, new { Cpf = cpf, DataInicial = dataInicial });
            });
        }
        public static string QuerySelectAllAgendamento()
        {
            return "SELECT a.id, emailUsuario, cpf_usuario AS CpfUsuario, id_quadra AS IdQuadra, status as StatusAgendamento, dataInicial as DataInicio, dataFinal as DataFim, " +
                "horario_inicial AS HorarioInicial, horario_final as HorarioFinal, presenca, recorrente, evento, diasSemana FROM agendamento a";
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
                      agendamento.IdQUadra,
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

        private static string GetQuery(Agendamento agendamento)
        {
            string sql;

            if (!agendamento.Evento && agendamento.Recorrente)
                throw new BadHttpRequestException("Agendamentos não podem ser recorrentes. Torne-o um Evento");

            if (agendamento.Evento && agendamento.Recorrente)
            {
                List<Agendamento> lista = GerarEventosSemanais(agendamento);
                sql = CriarComandosInsert(lista);
            }
            else
                sql = CriarComandosInsert(new List<Agendamento> { agendamento });

            return sql;
        }

        public static string CriarComandosInsert(List<Agendamento> eventos)
        {
            var comandos = new List<string>();

            foreach (var evento in eventos)
            {
                string diasSemanaArray = "{" + string.Join(",", evento.DiasSemana.Select(d => ((int)d).ToString())) + "}";


                string comando = "INSERT INTO agendamento (cpf_usuario, id_quadra, status, dataInicial, dataFinal, horario_inicial, horario_final, emailUsuario, presenca, evento, recorrente, diasSemana) " +
                                 "VALUES (" +
                                 $"'{evento.CpfUsuario}', " +
                                 $"{evento.IdQUadra}, " +
                                 $"CAST('{evento.StatusAgendamentoAux}' AS status_agendamento), " +
                                 $"'{evento.DataInicio.ToString("yyyy-MM-dd")}', " +
                                 $"'{evento.DataFim.GetValueOrDefault().ToString("yyyy-MM-dd")}', " +
                                 $"'{evento.HorarioInicial}', " +
                                 $"'{evento.HorarioFinal}', " +
                                 $"'{evento.EmailUsuario}', " +
                                 $"{(evento.Presenca ? "TRUE" : "FALSE")}, " +
                                 $"{(evento.Evento ? "TRUE" : "FALSE")}, " +
                                 $"{(evento.Recorrente ? "TRUE" : "FALSE")}, " +
                                 $"'{diasSemanaArray}'" +
                                 ")";

                if (eventos.Count == 1)
                    comando += " RETURNING *";
                else
                    comando += ";";

                comandos.Add(comando);
            }

            return string.Join(" ", comandos);
        }
    }
}
