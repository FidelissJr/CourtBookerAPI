using CourtBooker.Model;
using CourtBooker.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CourtBooker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;
        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> ListarUsuarios()
        {
            return await Task.Run(ActionResult<List<Usuario>> () =>
            {
                List<Usuario> result = _service.ListarUsuarios();
                return Ok(result);
            });
        }

        [HttpGet("{cpf}")]
        public async Task<ActionResult<Usuario>> BuscarUsuario(string cpf)
        {
            return await Task.Run(ActionResult<Usuario> () =>
            {
                Usuario? result = _service.BuscarUsuario(cpf);
                return Ok(result);
            });
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> AdicionarUsuario([FromBody] Usuario user)
        {
            return await Task.Run(ActionResult<Usuario> () =>
            {
                Usuario result = _service.AdicionarUsuario(user);
                return CreatedAtAction(nameof(AdicionarUsuario), result);
            });
        }

        [HttpPut]
        public async Task<IActionResult> EditarUsuario([FromBody] Usuario user)
        {
            return await Task.Run(IActionResult () =>
            {
                bool result = _service.EditarUsuario(user);
                return Ok(result);
            });
        }

        [HttpDelete("{cpf}")]
        public async Task<IActionResult> ExcluirUsuario(string cpf)
        {
            return await Task.Run(IActionResult () =>
            {
                bool result = _service.ExcluirUsuario(cpf);
                return Ok(result);
            });
        }
    }
}
