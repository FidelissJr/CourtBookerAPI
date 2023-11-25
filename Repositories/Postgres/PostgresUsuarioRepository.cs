using CourtBooker.Model;
using CourtBooker.Repositories.Interfaces;
using Dapper;

namespace CourtBooker.Repositories.Postgres
{
    public class PostgresUsuarioRepository : PostgresRepository, IUsuarioService
    {
        public List<Usuario> ListarUsuarios()
        {
            return WithConnection(dbConn =>
            {
                string sql = "SELECT cpf_usuario AS Cpf, Email, nome, hashsenha AS Senha, tipo as TipoUsuario, data_final_bolsa AS DataFimBolsa FROM usuario";
                return dbConn.Query<Usuario>(sql).ToList();
            });

        }
        public Usuario? BuscarUsuario(string cpf)
        {
            return WithConnection(dbConn =>
            {
                string sql = "SELECT cpf_usuario AS Cpf, Email, nome, hashsenha AS Senha, tipo as TipoUsuario, data_final_bolsa AS DataFimBolsa FROM usuario";
                sql += " WHERE cpf_usuario = @Cpf";
                return dbConn.QueryFirstOrDefault<Usuario>(sql, new { Cpf = cpf });
            });
        }

        public Usuario AdicionarUsuario(Usuario usuario)
        {
            return WithConnection(dbConn =>
            {
                string sql = "INSERT INTO usuario (cpf_usuario, nome, email, hashsenha, tipo, data_final_bolsa) VALUES (@Cpf, @Nome, @Email, @Senha, CAST(@TipoUsuarioAux AS tipo_usuario), @DataFimBolsa) RETURNING *";
                return dbConn.QuerySingle<Usuario>(sql, usuario);
            });
        }

        public bool EditarUsuario(Usuario user)
        {
            if (BuscarUsuario(user.Cpf) == null)
                throw new BadHttpRequestException("User not Found. Cpf is invalid");

            return WithConnection(dbConn =>
            {
                string sql = "UPDATE usuario SET nome = @Nome, hashsenha = @Senha, data_final_bolsa = @DataFimBolsa, email = @Email, tipo = CAST(@TipoUsuarioAux AS tipo_usuario) WHERE cpf_usuario = @Cpf";
                int rowsAffected = dbConn.Execute(sql, user);
                return rowsAffected > 0;
            });
        }

        public bool ExcluirUsuario(string cpf)
        {
            return WithConnection(dbConn =>
            {
                string sql = "Delete from usuario WHERE cpf_usuario = @Cpf";
                int rowsAffected = dbConn.Execute(sql, new { Cpf = cpf });
                return rowsAffected > 0;
            });
        }
    }
}
