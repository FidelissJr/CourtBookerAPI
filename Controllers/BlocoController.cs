using CourtBooker.Model;
using CourtBooker.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourtBooker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlocoController : ControllerBase
    {
        private readonly BlocoService _service;

        public BlocoController(BlocoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Bloco>>> ListarBlocos()
        {
            return await Task.Run(ActionResult<List<Bloco>> () =>
            {
                var result = _service.ListarBlocos();
                return Ok(result);
            });
        }
        [HttpPost]
        public async Task<ActionResult<Bloco>> AdicionarBloco([FromBody] Bloco bloco)
        {
            return await Task.Run(ActionResult<Bloco> () =>
            {
                var result = _service.AdicionarBloco(bloco);
                return CreatedAtAction(nameof(AdicionarBloco), result);
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirBloco(int id)
        {
            return await Task.Run(IActionResult () =>
            {
                var result = _service.ExcluirBloco(id);
                return Ok(result);
            });
        }
    }
}
