using CourtBooker.Model;
using CourtBooker.Repositories.Interfaces;
using Dapper;

namespace CourtBooker.Repositories.Postgres
{
    public class PostgresBlocoRepository : PostgresRepository, IBlocoService
    {
        public List<Bloco> ListarBlocos()
        {
            return WithConnection(dbConn =>
            {
                string sql = "SELECT * FROM bloco";
                return dbConn.Query<Bloco>(sql).ToList();
            });
        }

        public Bloco AdicionarBloco(Bloco bloco)
        {
            return WithConnection(dbConn =>
            {
                string sql = "INSERT INTO bloco (nome) VALUES (@Nome) RETURNING *";
                return dbConn.QuerySingle<Bloco>(sql, bloco);
            });
        }

        public bool ExcluirBloco(int id)
        {
            return WithConnection(dbConn =>
            {
                string sql = "Delete from bloco WHERE id = @Id";
                int rowsAffected = dbConn.Execute(sql, new { Id = id });
                return rowsAffected > 0;
            });
        }
    }
}
