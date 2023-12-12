using agendaback.Data;
using agendaback.ErrorHandling;
using agendaback.Model.GLPISistamas;
using agendaback.Model.GLPITI;
using agendaback.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace agendaback.Repository
{
    public class TicketUserGLPIRepository(ContextDBGLPITI contextDBGLPITI, ContextDBGLPISistemas contextDBGLPISistemas) : ITicketUserGLPIRepository
    {
        private readonly ContextDBGLPITI _contextDBGLPITI = contextDBGLPITI;
        private readonly ContextDBGLPISistemas _contextDBGLPISistemas = contextDBGLPISistemas;

        public async Task<List<TicketsUserGLPITI>> GetUserTicketGLPITIByIdTicket(int idTicket)
        {
            List<TicketsUserGLPITI> ticketUserGLPITI;
            try
            {
                ticketUserGLPITI = await _contextDBGLPITI.TicketsUserGLPITI.Where(u => u.Tickets_id == idTicket).ToListAsync();
                ticketUserGLPITI = ticketUserGLPITI.Where(u => u.Type == 2).ToList();
            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }
            return ticketUserGLPITI;
        }
        public async Task<List<TicketsUserGLPISistemas>> GetUserTicketGLPISistemasByIdTicket(int idTicket)
        {
            List<TicketsUserGLPISistemas> ticketUserGLPISistemas;
            try
            {
                ticketUserGLPISistemas = await _contextDBGLPISistemas.TicketsUserGLPISistemas.Where(u => u.Tickets_id == idTicket).ToListAsync();
                ticketUserGLPISistemas = ticketUserGLPISistemas.Where(u => u.Type == 2).ToList();
            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }
            return ticketUserGLPISistemas;
        }
    }
}

