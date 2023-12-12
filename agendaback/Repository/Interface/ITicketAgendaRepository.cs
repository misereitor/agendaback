using agendaback.Model.Agenda;
using agendaback.Model.Request;

namespace agendaback.Repository.Interface
{
    public interface ITicketAgendaRepository
    {
        Task<List<TicketAgenda>> GetAllTicketAgendas(int idUser);
        Task<TicketAgenda> GetTicketAgenda(int id, int userId);
        Task<TicketAgenda> InsertTicketAgenda(TicketAgendaRequest ticketAgenda);
        Task<TicketAgenda> UpdateTicketAgenda(TicketAgendaEdit ticket, int id);
        Task<bool> UpdateLockTicketAgenda(int idTicket);
        Task<bool> DeleteTicketAgenda(int id);
        Task<List<TicketAgenda>> GetAllTicketAgendas();
    }
}
