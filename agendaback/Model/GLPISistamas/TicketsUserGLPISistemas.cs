using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agendaback.Model.GLPISistamas
{
    [Table("glpi_tickets_users")]
    public class TicketsUserGLPISistemas
    {
        [Key]
        public int Id { get; set; }
        public int Tickets_id { get; set; }
        public int Users_id { get; set; }
        public int Type { get; set; }

        public TicketsUserGLPISistemas(int id, int tickets_id, int users_id, int type)
        {
            Id = id;
            Tickets_id = tickets_id;
            Users_id = users_id;
            Type = type;
        }

        public TicketsUserGLPISistemas()
        {
        }
    }
}
