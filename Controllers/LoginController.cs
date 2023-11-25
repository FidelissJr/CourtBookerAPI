using CourtBooker.Model;
using CourtBooker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourtBooker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly UsuarioService _usuarioService;
        public AuthController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }


        [HttpPost("login")]
        public IActionResult Login(string cpf, string senha)
        {
            Usuario? usuario = _usuarioService.BuscarUsuario(cpf);

            if (usuario != null)
            {
                if (!senha.Equals(usuario.Senha))
                    return Unauthorized(new { Message = "Senha inválida!" });

                return Ok();
            }

            return Unauthorized(new { Message = "Usuário não cadastrado!" });
        }

        [Authorize]
        [HttpGet("protected")]
        public IActionResult Protected()
        {
            return Ok("Você está autenticado!");
        }
    }

}
