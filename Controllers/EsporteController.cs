using CourtBooker.Model;
using CourtBooker.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourtBooker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EsporteController : ControllerBase
    {
        private readonly EsporteService _service;
        public EsporteController(EsporteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Esporte>>> ListarEsportes()
        {
            return await Task.Run(ActionResult<List<Esporte>> () =>
            {
                var result = _service.ListarEsportes();
                return Ok(result);
            });
        }

        [HttpPost]
        public async Task<ActionResult<Esporte>> AdicionarEsporte([FromBody] Esporte esporte)
        {
            return await Task.Run(ActionResult<Esporte> () =>
            {
                var result = _service.AdicionarEsporte(esporte);
                return CreatedAtAction(nameof(AdicionarEsporte), esporte);
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirEsporte(int id)
        {
            return await Task.Run(IActionResult () =>
            {
                var result = _service.ExcluirEsporte(id);
                return Ok(result);
            });
        }
    }
}
