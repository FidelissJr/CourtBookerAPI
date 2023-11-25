using CourtBooker.Model;
using CourtBooker.Repositories.Interfaces;
using Dapper;

namespace CourtBooker.Repositories.Postgres
{
    public class PostgresEsporteRepository : PostgresRepository, IEsporteService
    {
        public List<Esporte> ListarEsportes()
        {
            return WithConnection(dbConn =>
            {
                string sql = "SELECT * FROM tipoesporte";
                return dbConn.Query<Esporte>(sql).ToList();
            });

        }

        public Esporte AdicionarEsporte(Esporte esporte)
        {
            return WithConnection(dbConn =>
            {
                string sql = "INSERT INTO tipoesporte (nome) VALUES (@Nome) RETURNING *";
                return dbConn.QuerySingle<Esporte>(sql, esporte);
            });
        }

        public bool ExcluirEsporte(int id)
        {
            return WithConnection(dbConn =>
            {
                string sql = "Delete from tipoesporte WHERE id = @Id";
                int rowsAffected = dbConn.Execute(sql, new { Id = id });
                return rowsAffected > 0;
            });
        }
    }
}
