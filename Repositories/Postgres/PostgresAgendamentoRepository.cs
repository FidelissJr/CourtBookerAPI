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
                string sql = CriarComandosInsert(new List<Agendamento> { agendamento });
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

        public List<Agendamento> AdicionarEvento(List<Agendamento> agendamentos)
        {
            return WithConnection(dbConn =>
            {
                string sql = CriarComandosInsert(agendamentos);
                int rowsAffected = dbConn.Execute(sql);
                return agendamentos;
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
        public static string CriarComandosInsert(List<Agendamento> eventos)
        {
            var comandos = new List<string>();

            foreach (var evento in eventos)
            {
                string diasSemanaArray = "{" + string.Join(",", evento.DiasSemana.Select(d => ((int)d).ToString())) + "}";


                string comando = "INSERT INTO agendamento (cpf_usuario, id_quadra, status, dataInicial, dataFinal, horario_inicial, horario_final, emailUsuario, presenca, evento, recorrente, diasSemana) " +
                                 "VALUES (" +
                                 $"'{evento.CpfUsuario}', " +
                                 $"{evento.IdQuadra}, " +
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
