using agendaback.Data;
using agendaback.ErrorHandling;
using agendaback.Model.Agenda;
using agendaback.Model.Request;
using agendaback.Model.Response;
using agendaback.Repository.Interface;
using agendaback.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace agendaback.Repository
{
    public class TicketAgendaRepository(ContextDBAgenda contextDBAgenda, IUserAgendaRepository userAgenda,
            ITicketGLPIRepository ticketGLPIRepository, IHubContext<HubSocket> hubContext) : ITicketAgendaRepository
    {
        private readonly ContextDBAgenda _contextDBAgenda = contextDBAgenda;
        private readonly IUserAgendaRepository _userAgenda = userAgenda;
        private readonly ITicketGLPIRepository _ticketGLPIRepository = ticketGLPIRepository;
        private readonly IHubContext<HubSocket> _hubContext = hubContext;

        public async Task<bool> DeleteTicketAgenda(int id)
        {
            TicketAgenda ticket = await GetTicketAgenda(id);
            try
            {
                _contextDBAgenda.TicketAgenda.Remove(ticket);
                await _contextDBAgenda.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("ticketagenda", "ticketagendainsert");
                await _hubContext.Clients.All.SendAsync("ticketagenda", "ticketagenda");
            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }
            return true;
        }

        public async Task<List<TicketAgenda>> GetAllTicketAgendas(int idUser)
        {
            UserResponse user = await _userAgenda.GetUserAgendaById(idUser);
            List<TicketAgenda> tickets = [];

            if (user.Roles == "Technician")
            {
                try
                {
                    tickets = await _contextDBAgenda.TicketAgenda.Where(t => t.User_id_agenda == user.Id).ToListAsync();
                }
                catch (Exception ex)
                {
                    throw new ErrosException(500, ex.Message);
                }
            }
            else
            {
                tickets = await GetAllTicketAgendas();
            }
            return tickets;
        }

        public async Task<List<TicketAgenda>> GetAllTicketAgendas()
        {
            List<TicketAgenda> tickets = [];

            try
            {
                tickets = await _contextDBAgenda.TicketAgenda.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }
            return tickets;

        }

        public async Task<TicketAgenda> GetTicketAgenda(int id)
        {
            TicketAgenda? ticket = new();
            try
            {
                ticket = await _contextDBAgenda.TicketAgenda.FirstOrDefaultAsync(t => t.Id == id);
            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }
            return ticket ?? throw new ErrosException(404, "Chamado não encontrado");
        }
        public async Task<TicketAgenda> GetTicketAgenda(int id, int userId)
        {
            UserResponse user = await _userAgenda.GetUserAgendaById(userId);

            TicketAgenda? ticket = new();
            try
            {
                ticket = await _contextDBAgenda.TicketAgenda.FirstOrDefaultAsync(t => t.Id == id) ?? throw new ErrosException(404, "Chamado não encontrado");

            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }

            if (user.Roles == "Technician" && ticket.User_id_agenda != user.Id)
            {
                throw new ErrosException(403, "Usuário não tem permissão para essa função");
            }
            return ticket;
        }

        public async Task<TicketAgenda> InsertTicketAgenda(TicketAgendaRequest ticketAgenda)
        {
            TicketGLPIResponse ticketGLPI = await _ticketGLPIRepository.GetTicketGLPI(ticketAgenda);
            UserResponse user = await _userAgenda.GetUserAgendaById(ticketAgenda.Id_User);
            TicketAgenda? ticketExist = await _contextDBAgenda.TicketAgenda.FirstOrDefaultAsync(t => t.GLPI == ticketAgenda.GLPI && t.Ticket_id == ticketAgenda.Id_Ticket);
            int idGLPI = 0;
            if (ticketAgenda.GLPI == 1)
            {
                idGLPI = user.IdGLPITI;

            } else if (ticketAgenda.GLPI == 2)
            {
                idGLPI = user.IdGLPISistemas;
            }
            if (ticketExist == null)
            {
                await _hubContext.Clients.All.SendAsync("ticketagenda", "ticketagendainsert");
            }
            TicketAgenda ticket = new()
            {
                Ticket_id = ticketAgenda.Id_Ticket,
                User_id_glpi = idGLPI,
                User_id_agenda = user.Id,
                User_name = ticketGLPI.NameUser,
                Title = ticketGLPI.Name,
                Ticket_description = ticketGLPI.Content,
                Ticket_EntitieName = ticketGLPI.Entities,
                Date_criete = ticketGLPI.Date,
                Solve_date = ticketGLPI.SolveDate,
                Status = ticketGLPI.Status,
                Description = ticketGLPI.Content,
                Locations_id = ticketGLPI.Locations_id,
                Start = FormateDate(ticketAgenda.Start),
                End = FormateDate(ticketAgenda.End),
                GLPI = ticketAgenda.GLPI
            };
            try
            {
                await _contextDBAgenda.AddAsync(ticket);
                await _contextDBAgenda.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }
            await _hubContext.Clients.All.SendAsync("ticketagenda", "ticketagenda");

            return ticket;
        }

        public async Task<TicketAgenda> UpdateTicketAgenda(TicketAgendaEdit ticketEdit, int idUser)
        {
            UserResponse user = await _userAgenda.GetUserAgendaById(idUser);
            TicketAgenda ticket = await GetTicketAgenda(ticketEdit.Id);

            if (!ticket.Editable)
            {
                throw new ErrosException(403, "Ticket bloqueado!");
            }
            if (ticket.User_id_agenda == user.Id
                || user.Roles == "Administrator"
                || user.Roles == "Coordinator")
            {
                ticket.Start = FormateDate(ticketEdit.Start);
                ticket.End = FormateDate(ticketEdit.End);
                try
                {
                    _contextDBAgenda.TicketAgenda.Update(ticket);
                    await _contextDBAgenda.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new ErrosException(500, ex.Message);
                }
                await _hubContext.Clients.All.SendAsync("ticketagenda", "ticketagenda");
                return ticket;
            }
            throw new ErrosException(403, "usuário não tem permissão para essa ação!");
        }

        public async Task<bool> UpdateLockTicketAgenda(int idTicket)
        {
            TicketAgenda ticket = await GetTicketAgenda(idTicket);
            if (ticket.Status >= 5 && ticket.Editable) throw new ErrosException(401, "O chamado precisa ser reaberto para desbloquear");

            ticket.Editable = !ticket.Editable;
            try
            {
                _contextDBAgenda.TicketAgenda.Update(ticket);
                await _contextDBAgenda.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("ticketagenda", "ticketagenda");
            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }
            return ticket.Editable;
        }

        private static DateTime FormateDate(DateTime date)
        {
            if (date.Kind != DateTimeKind.Utc)
            {
                // Se o Kind não for DateTimeKind.Utc, convertemos para DateTimeKind.Utc
                date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
            }

            TimeZoneInfo brasiliaZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            DateTime brasiliaTime = TimeZoneInfo.ConvertTimeFromUtc(date, brasiliaZone);
            return brasiliaTime;
        }

    }
}
