using CourtBooker.Model;

namespace CourtBooker.Repositories.Interfaces
{
    public interface IUsuarioService
    {
        public List<Usuario> ListarUsuarios();
        public Usuario BuscarUsuario(string cpf);
        public Usuario AdicionarUsuario(Usuario user);
        public bool EditarUsuario(Usuario user);
        public bool ExcluirUsuario(string cpf);

    }
}
