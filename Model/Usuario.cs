using CourtBooker.Enuns;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CourtBooker.Model
{
    [Table("usuario")]
    public class Usuario
    {
        [StringLength(11, ErrorMessage = "Formato do Cpf inválido", MinimumLength = 11)]
        [Required(ErrorMessage = "O campo Cpf é obrigatório e não pode ser deixado em branco")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório e não pode ser deixado em branco")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório e não pode ser deixado em branco")]
        public string Email { get; set; }

        public DateTime? DataFimBolsa { get; set; }

        [Range(0, 2)]
        public TipoUsuario TipoUsuario { get; set; }
        public string TipoUsuarioAux => TipoUsuario.ToString().ToLower();

        [Required(ErrorMessage = "O campo Nome é obrigatório e não pode ser deixado em branco")]
        public string Nome { get; set; }


        public Usuario(string cpf, string senha, string email, TipoUsuario tipoUsuario, DateTime? dataFimBolsa)
        {
            Cpf = cpf;
            Senha = senha;
            Email = email;
            TipoUsuario = tipoUsuario;
            DataFimBolsa = dataFimBolsa;
        }

        public Usuario()
        {

        }
    }
}
