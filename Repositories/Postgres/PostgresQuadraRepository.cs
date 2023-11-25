using CourtBooker.Model;
using CourtBooker.Repositories.Interfaces;
using Dapper;

namespace CourtBooker.Repositories.Postgres
{
    public class PostgresQuadraRepository : PostgresRepository, IQuadraService
    {
        public List<Quadra> ListarQuadras()
        {
            return WithConnection(dbConn =>
            {
                string sql = QuerySelectAllAgendamento();
                return dbConn.Query<Quadra>(sql).ToList();
            });
        }
        public Quadra? BuscarQuadra(int id)
        {
            return WithConnection(dbConn =>
            {
                string sql = QuerySelectAllAgendamento();
                sql += " WHERE id = @Id";
                return dbConn.QueryFirstOrDefault<Quadra>(sql, new { Id = id });
            });
        }

        public Quadra AdicionarQuadra(Quadra quadra)
        {
            return WithConnection(dbConn =>
            {
                string sql = "INSERT INTO quadra (nome, id_bloco) VALUES (@Nome, @IdBloco) RETURNING *";
                return dbConn.QuerySingle<Quadra>(sql, quadra);
            });
        }

        public bool AdicionarQuadraEsporte(int idQuadra, int idEsporte)
        {
            return WithConnection(dbConn =>
            {
                string sql = "INSERT INTO quadra_tipoesporte (id_quadra, id_tipo_esporte) VALUES (@IdQuadra, @IdEsporte)";
                int rowsAffected = dbConn.Execute(sql, new { IdQuadra = idQuadra, IdEsporte = idEsporte });
                return rowsAffected > 0;
            });
        }

        public bool ExcluirQuadraEsporte(int idQuadra, int idEsporte)
        {
            return WithConnection(dbConn =>
            {
                string sql = "Delete FROM quadra_tipoesporte WHERE id_quadra = @IdQuadra AND id_tipo_esporte = @IdEsporte";
                int rowsAffected = dbConn.Execute(sql, new { IdQuadra = idQuadra, IdEsporte = idEsporte });
                return rowsAffected > 0;
            });
        }

        public bool ExcluirQuadra(int id)
        {
            return WithConnection(dbConn =>
            {
                string sql = "Delete from quadra WHERE id = @Id";
                int rowsAffected = dbConn.Execute(sql, new { Id = id });
                return rowsAffected > 0;
            });
        }

        public static string QuerySelectAllAgendamento()
        {
            return "SELECT q.nome, q.id, q.id_bloco as idBloco, qte.idTiposEsporte FROM quadra q LEFT JOIN " +
                "(SELECT id_quadra, array_agg(id_tipo_esporte) AS idTiposEsporte FROM quadra_tipoesporte " +
                "GROUP BY id_quadra) qte ON qte.id_quadra = q.id";
        }
    }
}
