using agendaback.ErrorHandling;
using agendaback.Model.Agenda;
using agendaback.Model.Request;
using agendaback.Model.Response;
using agendaback.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace agendaback.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController(ITicketAgendaRepository ticketAgendaRepository, ITicketGLPIRepository ticketGLPIRepository) : ControllerBase
    {
        private readonly ITicketAgendaRepository _ticketAgendaRepository = ticketAgendaRepository;
        private readonly ITicketGLPIRepository _ticketGLPIRepository = ticketGLPIRepository;

        [HttpGet("getallticketAgenda")]
        [Authorize]
        public async Task<ActionResult<List<TicketAgenda>>> GetAllTicketAgenda()
        {
            List<TicketAgenda> tickets;
            var userIdClaim = User.FindFirst(ClaimTypes.Locality);
            if (userIdClaim != null)
            {
                int userId = Int32.Parse(userIdClaim.Value);
                tickets = await _ticketAgendaRepository.GetAllTicketAgendas(userId);
                return Ok(tickets);
            }
            throw new ErrosException(403, "Usuáro não autenticado");
        }

        [HttpGet("getallticketglpi")]
        [Authorize]
        public async Task<ActionResult<List<TicketGLPIResponse>>> GetAllTicketGLPI()
        {
            List<TicketGLPIResponse> tickets;
            var userIdClaim = User.FindFirst(ClaimTypes.Locality);
            if (userIdClaim != null)
            {
                int userId = Int32.Parse(userIdClaim.Value);
                tickets = await _ticketGLPIRepository.GetAllTicketGLPI(userId);
                return Ok(tickets);
            }
            throw new ErrosException(403, "Usuáro não autenticado");
        }

        [HttpGet("getticketglpi")]
        [Authorize]
        public async Task<ActionResult<TicketGLPIResponse>> GetTicketGLPIById(TicketAgendaRequest ticketAgendaRequest)
        {
            TicketGLPIResponse ticket = await _ticketGLPIRepository.GetTicketGLPI(ticketAgendaRequest);
            return Ok(ticket);
        }

        [HttpGet("getticketagenda/{id}")]
        [Authorize]
        public async Task<ActionResult<TicketAgenda>> GetTicketAgendaById(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.Locality);
            if (userIdClaim != null)
            {
                int userId = Int32.Parse(userIdClaim.Value);
                TicketAgenda ticket = await _ticketAgendaRepository.GetTicketAgenda(id, userId);
                return Ok(ticket);
            }
            throw new ErrosException(403, "Usuáro não autenticado");
        }

        [HttpPost("insertticketagenda")]
        [Authorize(Roles = "Administrator, Coordinator, Technician")]
        public async Task<ActionResult<TicketAgenda>> InsertTicketAgenda([FromBody] TicketAgendaRequest ticketAgenda)
        {
            TicketAgenda ticket = await _ticketAgendaRepository.InsertTicketAgenda(ticketAgenda);
            return Ok(ticket);
        }

        [HttpPut("alterticketagenda")]
        [Authorize(Roles = "Administrator, Coordinator, Technician")]
        public async Task<ActionResult<TicketAgenda>> PutTicketAgenda(TicketAgendaEdit ticketAgenda)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.Locality);
            if (userIdClaim != null)
            {
                int userId = Int32.Parse(userIdClaim.Value);
                TicketAgenda ticket = await _ticketAgendaRepository.UpdateTicketAgenda(ticketAgenda, userId);
                return Ok(ticket);
            }
            throw new ErrosException(403, "Usuáro não autenticado");
        }

        [HttpPut("updatelockticketagenda/{id}")]
        [Authorize(Roles = "Administrator, Coordinator")]
        public async Task<ActionResult<bool>> UpdateLockTicketAgenda(int id)
        {
            bool locked = await _ticketAgendaRepository.UpdateLockTicketAgenda(id);
            return Ok(locked);
        }

        [HttpDelete("deleteticketaagnda/{id}")]
        [Authorize(Roles = "Administrator, Coordinator")]
        public async Task<ActionResult<bool>> DeleteTicketAgenda(int id)
        {
            bool delete = await _ticketAgendaRepository.DeleteTicketAgenda(id);
            return Ok(delete);
        }
    }
}
