using CourtBooker.Model;
using CourtBooker.Repositories.Interfaces;

namespace CourtBooker.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioService _repository;

        public UsuarioService(IUsuarioService repository)
        {
            _repository = repository;
        }
        public List<Usuario> ListarUsuarios()
        {
            return _repository.ListarUsuarios();
        }
        public Usuario? BuscarUsuario(string cpf)
        {
            return _repository.BuscarUsuario(cpf);
        }

        public Usuario AdicionarUsuario(Usuario usuario)
        {
            return _repository.AdicionarUsuario(usuario);
        }

        public bool EditarUsuario(Usuario usuario)
        {
            if (BuscarUsuario(usuario.Cpf) == null)
                throw new BadHttpRequestException("Usuário nao encontrado. CPF não cadastrado");

            return _repository.EditarUsuario(usuario);
        }

        public bool ExcluirUsuario(string cpf)
        {
            return _repository.ExcluirUsuario(cpf);
        }
    }
}
