using CourtBooker.Enuns;
using CourtBooker.Model;
using CourtBooker.Repositories.Interfaces;
using CourtBooker.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourtBooker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendamentoController : ControllerBase
    {
        private readonly IEmailSender _emailSender;
        private readonly AgendamentoService _service;

        public AgendamentoController(IEmailSender emailSender, AgendamentoService service)
        {
            _emailSender = emailSender;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Agendamento>>> ListarAgendamentos()
        {
            return await Task.Run(ActionResult<List<Agendamento>> () =>
            {
                List<Agendamento> result = _service.ListarAgendamentos();
                return Ok(result);
            });
        }

        [HttpGet("AgendamentosDoBloco/{idBloco}")]
        public async Task<ActionResult<List<Agendamento>>> ListarAgendamentosBloco(int idBloco)
        {
            return await Task.Run(ActionResult<List<Agendamento>> () =>
            {
                List<Agendamento> result = _service.ListarAgendamentosBloco(idBloco);
                return Ok(result);
            });
        }

        [HttpPost]
        public async Task<ActionResult<Agendamento>> AdicionarAgendamento([FromBody] Agendamento agendamento)
        {
            return await Task.Run(ActionResult<Agendamento> () =>
            {
                _service.ValidarAgendamento(agendamento);
                _service.GetEmailMessage(agendamento, out string message, out string receiver, out string subject, false);
                _emailSender.SendEmailAsync(receiver, subject, message);
                return CreatedAtAction(nameof(AdicionarAgendamento), new {agendamento});
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirAgendamento(int id)
        {
            return await Task.Run(IActionResult () =>
            {
                Agendamento agendamento = _service.BuscarAgendamento(id);
                bool result = _service.ExcluirAgendamento(id);
                _service.GetEmailMessage(agendamento, out string message, out string receiver, out string subject, true);
                _emailSender.SendEmailAsync(receiver, subject, message);
                return Ok(result);
            });
        }

        [HttpGet("ListarDiasSemana")]
        public async Task<ActionResult<List<EnumValueDescription>>> ListarDiasSemana()
        {
            return await Task.Run(ActionResult<List<EnumValueDescription>> () =>
            {
                var result = _service.ListarDiasSemana();
                return Ok(result);
            });
        }
    }
}
