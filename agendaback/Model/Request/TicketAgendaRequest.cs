using System.ComponentModel.DataAnnotations;

namespace agendaback.Model.Request
{
    public class TicketAgendaRequest
    {
        [Required]
        public int Id_User { get; set; }
        [Required]
        public int Id_Ticket { get; set; }
        [Required]
        public int GLPI { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public TicketAgendaRequest(int id_User, int id_Ticket, int glpi, DateTime start, DateTime end)
        {
            Id_User = id_User;
            Id_Ticket = id_Ticket;
            GLPI = glpi;
            Start = start;
            End = end;
        }

        public TicketAgendaRequest()
        {
        }
    }
}
