using agendaback.Data;
using agendaback.ErrorHandling;
using agendaback.Model.Agenda;
using agendaback.Model.GLPISistamas;
using agendaback.Model.GLPITI;
using agendaback.Model.Request;
using agendaback.Model.Response;
using agendaback.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace agendaback.Repository
{

    public class TicketGLPIRepository(ContextDBGLPITI contextDBGLPITI, IUserAgendaRepository userAgenda,
        ContextDBAgenda contextDBAgenda, ContextDBGLPISistemas contextDBGLPISistemas) : ITicketGLPIRepository
    {
        private readonly IUserAgendaRepository _userAgenda = userAgenda;
        private readonly ContextDBGLPITI _contextDBGLPITI = contextDBGLPITI;
        private readonly ContextDBGLPISistemas _contextDBGLPISistemas = contextDBGLPISistemas;
        private readonly ContextDBAgenda _contextDBAgenda = contextDBAgenda;

        public async Task<List<TicketGLPIResponse>> GetAllTicketGLPI(int idUser)
        {
            UserResponse user = await _userAgenda.GetUserAgendaById(idUser);
            if (user.Roles == "Technician")
            {
                return await GetAllTicketGLPIByUserId(user);
            }
            else
            {
                return await GetAllTicketGLPI();
            }
        }

        public async Task<List<TicketGLPIResponse>> GetAllTicketGLPI()
        {
            List<TicketGLPIResponse> ticketDetails = [];
            List<TicketsUserGLPITI> usersGLPITI = [];
            List<TicketsUserGLPISistemas> usersGLPISistemas = [];
            List<TicketGLPITI> ticketsGLPITI;
            List<TicketGLPISistemas> ticketsGLPISistemas;
            List<EntitiesGLPITI> entitiesGLPITI = [];
            List<EntitiesGLPISistemas> entitiesGLPISistemas = [];
            List<UserGLPITI> userGLPITI = [];
            List<UserGLPISistemas> userGLPISistemas = [];
            List<UserAgenda> userAgenda = [];
            List<TicketAgenda> ticketAgenda = [];
            try
            {
                ticketsGLPITI = await _contextDBGLPITI.TicketGLPITI.Where(t => t.Status < 5).ToListAsync();
                ticketsGLPISistemas = await _contextDBGLPISistemas.TicketGLPISistemas.Where(t => t.Status < 5).ToListAsync();
                usersGLPITI = await _contextDBGLPITI.TicketsUserGLPITI.Where(tu => tu.Type == 2).ToListAsync();
                usersGLPISistemas = await _contextDBGLPISistemas.TicketsUserGLPISistemas.Where(tu => tu.Type == 2).ToListAsync();
                entitiesGLPITI = await _contextDBGLPITI.EntitiesGLPITI.ToListAsync();
                entitiesGLPISistemas = await _contextDBGLPISistemas.EntitiesGLPISistemas.ToListAsync();
                userGLPITI = await _contextDBGLPITI.UserGLPITI.ToListAsync();
                userGLPISistemas = await _contextDBGLPISistemas.UserGLPISistemas.ToListAsync();
                userAgenda = await _contextDBAgenda.UserAgenda.ToListAsync();
                ticketAgenda = await _contextDBAgenda.TicketAgenda.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }

            foreach (var ticket in ticketsGLPITI)
            {
                TicketAgenda? exist = ticketAgenda.FirstOrDefault(t => t.GLPI == 1 && t.Ticket_id == ticket.Id);
                if (exist == null)
                {
                    List<TicketsUserGLPITI> usersInTicket = [];
                    usersInTicket = usersGLPITI.Where(t => t.Tickets_id == ticket.Id).ToList();

                    foreach (var user in usersInTicket)
                    {
                        if (entitiesGLPITI == null) continue;
                        if (userGLPITI == null) continue;
                        if (userAgenda == null) continue;
                        UserGLPITI? userGLPITIInTicket = new();
                        UserAgenda? userAgendaInTicket = new();

                        EntitiesGLPITI? entitiesInTicket = entitiesGLPITI.FirstOrDefault(e => e.Id == ticket.Entities_id);
                        if (entitiesInTicket == null) continue;

                        userGLPITIInTicket = userGLPITI.FirstOrDefault(u => u.Id == user.Users_id);
                        if (userGLPITIInTicket == null) continue;

                        userAgendaInTicket = userAgenda.FirstOrDefault(u => u.IdGLPITI == userGLPITIInTicket.Id);
                        if (userAgendaInTicket == null) continue;

                        if (userGLPITIInTicket != null && entitiesInTicket != null)
                        {
                            var ticketResponse = new TicketGLPIResponse
                            {
                                Id = ticket.Id,
                                Entities_id = ticket.Entities_id,
                                User_agenda_id = userAgendaInTicket.Id,
                                Name = ticket.Name,
                                Date = ticket.Date,
                                SolveDate = ticket.SolveDate,
                                Status = ticket.Status,
                                Content = ticket.Content,
                                Type = ticket.Type,
                                ActionTime = ticket.ActionTime,
                                Locations_id = ticket.Locations_id,
                                User_id = user.Users_id,
                                Entities = entitiesInTicket.Name,
                                NameUser = userGLPITIInTicket.Name,
                                GLPI = 1
                            };
                            ticketDetails.Add(ticketResponse);
                        }
                    }
                }
            }

            foreach (var ticket in ticketsGLPISistemas)
            {
                TicketAgenda? exist = ticketAgenda.FirstOrDefault(t => t.GLPI == 2 && t.Ticket_id == ticket.Id);

                if (exist == null)
                {
                    List<TicketsUserGLPISistemas> usersInTicket = [];
                    usersInTicket = usersGLPISistemas.Where(t => t.Tickets_id == ticket.Id).ToList();

                    foreach (var user in usersInTicket)
                    {
                        if (entitiesGLPISistemas == null) continue;
                        if (userGLPISistemas == null) continue;
                        if (userAgenda == null) continue;
                        UserGLPISistemas? userGLPISistemasInTicket = new();
                        UserAgenda? userAgendaInTicket = new();

                        EntitiesGLPISistemas? entitiesInTicket = entitiesGLPISistemas.FirstOrDefault(e => e.Id == ticket.Entities_id);
                        if (entitiesInTicket == null) continue;

                        userGLPISistemasInTicket = userGLPISistemas.FirstOrDefault(u => u.Id == user.Users_id);
                        if (userGLPISistemasInTicket == null) continue;

                        userAgendaInTicket = userAgenda.FirstOrDefault(u => u.IdGLPISistemas == userGLPISistemasInTicket.Id);
                        if (userAgendaInTicket == null) continue;

                        if (userGLPISistemasInTicket != null && entitiesInTicket != null)
                        {
                            var ticketResponse = new TicketGLPIResponse
                            {
                                Id = ticket.Id,
                                Entities_id = ticket.Entities_id,
                                User_agenda_id = userAgendaInTicket.Id,
                                Name = ticket.Name,
                                Date = ticket.Date,
                                SolveDate = ticket.SolveDate,
                                Status = ticket.Status,
                                Content = ticket.Content,
                                Type = ticket.Type,
                                ActionTime = ticket.ActionTime,
                                Locations_id = ticket.Locations_id,
                                User_id = user.Users_id,
                                Entities = entitiesInTicket.Name,
                                NameUser = userGLPISistemasInTicket.Name,
                                GLPI = 2
                            };
                            ticketDetails.Add(ticketResponse);
                        }
                    }
                }
            }
            return ticketDetails;
        }

        private async Task<List<TicketGLPIResponse>> GetAllTicketGLPIByUserId(UserResponse user)
        {
            List<TicketGLPIResponse> ticketDetails = [];
            List<EntitiesGLPITI> entitiesGLPITI = [];
            List<EntitiesGLPISistemas> entitiesGLPISistemas = [];
            UserAgenda? userAgenda = new();
            List<TicketGLPITI> ticketsGLPITI = [];
            List<TicketGLPISistemas> ticketsGLPISistemas = [];
            List<TicketGLPITI> ticketsUserGLPITI = [];
            List<TicketGLPISistemas> ticketsUserGLPISistemas = [];
            List<TicketsUserGLPITI> userAgendaGLPITIInTicket = [];
            List<TicketsUserGLPISistemas> userAgendaGLPISistemasInTicket = [];
            List<TicketAgenda> ticketAgenda = [];
            try
            {
                userAgenda = await _contextDBAgenda.UserAgenda.FirstOrDefaultAsync(tu => tu.Id == user.Id);
                entitiesGLPITI = await _contextDBGLPITI.EntitiesGLPITI.ToListAsync();
                entitiesGLPISistemas = await _contextDBGLPISistemas.EntitiesGLPISistemas.ToListAsync();
                userAgendaGLPITIInTicket = await _contextDBGLPITI.TicketsUserGLPITI.Where(u => u.Type == 2 && u.Users_id == user.IdGLPITI).ToListAsync();
                userAgendaGLPISistemasInTicket = await _contextDBGLPISistemas.TicketsUserGLPISistemas.Where(u => u.Type == 2 && u.Users_id == user.IdGLPISistemas).ToListAsync();
                ticketAgenda = await _contextDBAgenda.TicketAgenda.Where(t => t.User_id_agenda == user.Id).ToListAsync();
                ticketsGLPITI = await _contextDBGLPITI.TicketGLPITI.Where(t => t.Status < 5).ToListAsync();
                ticketsGLPISistemas = await _contextDBGLPISistemas.TicketGLPISistemas.Where(t => t.Status < 5).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }
            if (userAgenda == null || userAgendaGLPITIInTicket == null || userAgendaGLPISistemasInTicket == null) throw new ErrosException(404, "Usuário não encontrado");
            foreach (var tu in userAgendaGLPITIInTicket)
            {
                var tic = ticketsGLPITI.FirstOrDefault(t => t.Id == tu.Tickets_id);
                if (tic == null) continue;
                ticketsUserGLPITI.Add(tic);
            }
            foreach (var tu in userAgendaGLPISistemasInTicket)
            {
                var tic = ticketsGLPISistemas.FirstOrDefault(t => t.Id == tu.Tickets_id);
                if (tic == null) continue;
                ticketsUserGLPISistemas.Add(tic);
            }
            foreach (var ticket in ticketsUserGLPITI)
            {
                TicketAgenda? exist = ticketAgenda.FirstOrDefault(t => t.GLPI == 1 && t.Ticket_id == ticket.Id);
                if (exist == null)
                {

                    EntitiesGLPITI? entitiesInTicket = entitiesGLPITI.FirstOrDefault(e => e.Id == ticket.Entities_id);
                    if (entitiesInTicket == null) continue;

                    if (entitiesInTicket != null)
                    {
                        var ticketResponse = new TicketGLPIResponse
                        {
                            Id = ticket.Id,
                            Entities_id = ticket.Entities_id,
                            User_agenda_id = userAgenda.Id,
                            Name = ticket.Name,
                            Date = ticket.Date,
                            SolveDate = ticket.SolveDate,
                            Status = ticket.Status,
                            Content = ticket.Content,
                            Type = ticket.Type,
                            ActionTime = ticket.ActionTime,
                            Locations_id = ticket.Locations_id,
                            User_id = user.IdGLPITI,
                            Entities = entitiesInTicket.Name,
                            NameUser = userAgenda.UserName,
                            GLPI = 1
                        };

                        ticketDetails.Add(ticketResponse);
                    }
                }

            }
            foreach (var ticket in ticketsUserGLPISistemas)
            {
                TicketAgenda? exist = ticketAgenda.FirstOrDefault(t => t.GLPI == 2 && t.Ticket_id == ticket.Id);
                if (exist == null)
                {

                    EntitiesGLPISistemas? entitiesInTicket = entitiesGLPISistemas.FirstOrDefault(e => e.Id == ticket.Entities_id);
                    if (entitiesInTicket == null) continue;

                    if (entitiesInTicket != null)
                    {
                        var ticketResponse = new TicketGLPIResponse
                        {
                            Id = ticket.Id,
                            Entities_id = ticket.Entities_id,
                            User_agenda_id = userAgenda.Id,
                            Name = ticket.Name,
                            Date = ticket.Date,
                            SolveDate = ticket.SolveDate,
                            Status = ticket.Status,
                            Content = ticket.Content,
                            Type = ticket.Type,
                            ActionTime = ticket.ActionTime,
                            Locations_id = ticket.Locations_id,
                            User_id = user.IdGLPISistemas,
                            Entities = entitiesInTicket.Name,
                            NameUser = userAgenda.UserName,
                            GLPI = 2
                        };

                        ticketDetails.Add(ticketResponse);
                    }
                }

            }
            return ticketDetails;
        }

        public async Task<TicketGLPIResponse> GetTicketGLPI(TicketAgendaRequest ticketAgendaRequest)
        {
            UserResponse userAgenda = await _userAgenda.GetUserAgendaById(ticketAgendaRequest.Id_User);

            //TicketAgenda ticketAgenda = await _contextDBAgenda.TicketAgenda.FirstOrDefaultAsync(t => t.Id == ticketAgendaRequest.Id_Ticket) ??
            //   throw new ErrosException(404, "Chamado não encontrado");

            if (ticketAgendaRequest.GLPI == 1)
            {
                TicketGLPITI? ticket = new();
                TicketsUserGLPITI? ticketUserGLPITI = new();
                UserGLPITI? userGLPI = new();
                EntitiesGLPITI? entities = new();

               try
                {
                    ticket = await _contextDBGLPITI.TicketGLPITI.FirstOrDefaultAsync(t => t.Id == ticketAgendaRequest.Id_Ticket) ??
                   throw new ErrosException(404, "Chamado não encontrado");
                    ticketUserGLPITI = await _contextDBGLPITI.TicketsUserGLPITI
                        .FirstOrDefaultAsync(u => u.Users_id == userAgenda.IdGLPITI && u.Tickets_id == ticketAgendaRequest.Id_Ticket) ??
                        throw new ErrosException(404, "O chamado não precence a esse usuário");
                    userGLPI = await _contextDBGLPITI.UserGLPITI.FirstOrDefaultAsync(u => u.Id == userAgenda.IdGLPITI) ??
                        throw new ErrosException(404, "Usuario não encontrado");
                    entities = await _contextDBGLPITI.EntitiesGLPITI
                            .FirstOrDefaultAsync(e => e.Id == ticket.Entities_id) ??
                                throw new ErrosException(404, "Empresa não encontrada");
                }
                catch (Exception ex)
                {
                    throw new ErrosException(500, ex.Message);
                }
                TicketGLPIResponse ticketDetails = new()
                {
                    Id = ticket.Id,
                    Entities_id = ticket.Entities_id,
                    User_agenda_id = userAgenda.Id,
                    Name = ticket.Name,
                    Date = ticket.Date,
                    SolveDate = ticket.SolveDate,
                    Status = ticket.Status,
                    Content = ticket.Content,
                    Type = ticket.Type,
                    ActionTime = ticket.ActionTime,
                    Locations_id = ticket.Locations_id,
                    User_id = ticketAgendaRequest.Id_User,
                    Entities = entities.Name,
                    NameUser = userGLPI.Name,
                    GLPI = 1
                };
                return ticketDetails;
            }
            else if (ticketAgendaRequest.GLPI == 2)
            {
                TicketGLPISistemas? ticket = new();
                TicketsUserGLPISistemas? ticketUserGLPITI = new();
                UserGLPISistemas? userGLPI = new();
                EntitiesGLPISistemas? entities = new();

                try
                {
                    ticket = await _contextDBGLPISistemas.TicketGLPISistemas.FirstOrDefaultAsync(t => t.Id == ticketAgendaRequest.Id_Ticket) ??
                    throw new ErrosException(404, "Chamado não encontrado");
                    ticketUserGLPITI = await _contextDBGLPISistemas.TicketsUserGLPISistemas
                        .FirstOrDefaultAsync(u => u.Users_id == userAgenda.IdGLPISistemas && u.Tickets_id == ticketAgendaRequest.Id_Ticket) ??
                        throw new ErrosException(404, "O chamado não precence a esse usuário");
                    userGLPI = await _contextDBGLPISistemas.UserGLPISistemas.FirstOrDefaultAsync(u => u.Id == userAgenda.IdGLPISistemas) ??
                        throw new ErrosException(404, "Usuario não encontrado");
                    entities = await _contextDBGLPISistemas.EntitiesGLPISistemas
                            .FirstOrDefaultAsync(e => e.Id == ticket.Entities_id) ??
                                throw new ErrosException(404, "Empresa não encontrada");
                }
                catch (Exception ex)
                {
                    throw new ErrosException(500, ex.Message);
                }
                TicketGLPIResponse ticketDetails = new()
                {
                    Id = ticket.Id,
                    Entities_id = ticket.Entities_id,
                    User_agenda_id = userAgenda.Id,
                    Name = ticket.Name,
                    Date = ticket.Date,
                    SolveDate = ticket.SolveDate,
                    Status = ticket.Status,
                    Content = ticket.Content,
                    Type = ticket.Type,
                    ActionTime = ticket.ActionTime,
                    Locations_id = ticket.Locations_id,
                    User_id = ticketAgendaRequest.Id_User,
                    Entities = entities.Name,
                    NameUser = userGLPI.Name,
                    GLPI = 2
                };
                return ticketDetails;
            }
            throw new ErrosException(404, "Chamado não encontrado");
        }


        private static bool CheckTicketInAgenda(int idTicket, List<TicketAgenda> ticketAgenda)
        {
            TicketAgenda? exist = ticketAgenda.FirstOrDefault(t => t.Ticket_id == idTicket);
            if (exist == null)
            {
                return false;
            }
            return false;
        }

    }
    
}
