using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agendaback.Model.GLPISistamas
{
    [Table("glpi_tickets")]
    public class TicketGLPISistemas
    {
        [Key]
        public int Id { get; set; }
        public int Entities_id { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? SolveDate { get; set; }
        public int Status { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public int ActionTime { get; set; }
        public int Locations_id { get; set; }

        public TicketGLPISistemas(int id, int entities_id, string name, DateTime date, DateTime solveDate, int status, string content, int type, int actionTime, int locations_id)
        {
            Id = id;
            Entities_id = entities_id;
            Name = name;
            Date = date;
            SolveDate = solveDate;
            Status = status;
            Content = content;
            Type = type;
            ActionTime = actionTime;
            Locations_id = locations_id;
        }

        public TicketGLPISistemas()
        {
            Name = string.Empty;
            Content = string.Empty;
        }
    }
}
