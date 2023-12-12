using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace agendaback.Model.Agenda
{
    [Table("agenda_tickets")]
    public class TicketAgenda
    {
        [Key]
        public int Id { get; set; }
        public int Ticket_id { get; set; }
        public int User_id_glpi { get; set; }
        public int User_id_agenda { get; set; }
        public string User_name { get; set; }
        public string Title { get; set; }
        public string Ticket_description { get; set; }
        public string Ticket_EntitieName { get; set; }
        public DateTime? Date_criete { get; set; }
        public DateTime? Solve_date { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public int Locations_id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        [DefaultValue(true)]
        public bool Editable { get; set; }
        [Required]
        public int GLPI { get; set; }

        public TicketAgenda(int id, int ticket_id, int user_id_glpi, int user_id_agenda, string user_name,
            string title, string ticket_description, string ticket_EntitieName, DateTime? date_criete,
            DateTime? solve_date, int status, string description, int locations_id, DateTime start,
            DateTime end, bool editable, int glpi)
        {
            Id = id;
            Ticket_id = ticket_id;
            User_id_glpi = user_id_glpi;
            User_id_agenda = user_id_agenda;
            User_name = user_name;
            Title = title;
            Ticket_description = ticket_description;
            Ticket_EntitieName = ticket_EntitieName;
            Date_criete = date_criete;
            Solve_date = solve_date;
            Status = status;
            Description = description;
            Locations_id = locations_id;
            Start = start;
            End = end;
            Editable = editable;
            GLPI = glpi;
        }

        public TicketAgenda()
        {
            User_name = string.Empty;
            Title = string.Empty;
            Ticket_description = string.Empty;
            Description = string.Empty;
            Ticket_EntitieName = string.Empty;
            Editable = true;
        }
    }
}
