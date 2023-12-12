using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace agendaback.Model.GLPITI
{
    [Table("glpi_tickets")]
    public class TicketGLPITI
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

        public TicketGLPITI(int id, int entities_id, string name, DateTime date, DateTime solveDate, int status, string content, int type, int actionTime, int locations_id)
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

        public TicketGLPITI()
        {
            Name = string.Empty;
            Content = string.Empty;
        }
    }
}
