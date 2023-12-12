using agendaback.Model.GLPISistamas;
using agendaback.Model.GLPITI;

namespace agendaback.Repository.Interface
{
    public interface ITicketUserGLPIRepository
    {
        Task<List<TicketsUserGLPITI>> GetUserTicketGLPITIByIdTicket(int idTicket);
        Task<List<TicketsUserGLPISistemas>> GetUserTicketGLPISistemasByIdTicket(int idTicket);
    }
}
