using agendaback.Model.Request;
using agendaback.Model.Response;

namespace agendaback.Repository.Interface
{
    public interface ITicketGLPIRepository
    {
        Task<List<TicketGLPIResponse>> GetAllTicketGLPI(int idUser);
        Task<List<TicketGLPIResponse>> GetAllTicketGLPI();
        Task<TicketGLPIResponse> GetTicketGLPI(TicketAgendaRequest ticketAgendaRequest);
    }
}
