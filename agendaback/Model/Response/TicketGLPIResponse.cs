using System.ComponentModel.DataAnnotations;

namespace agendaback.Model.Response
{
    public class TicketGLPIResponse
    {
        [Key]
        public int Id { get; set; }
        public int Entities_id { get; set; }
        public int User_agenda_id { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? SolveDate { get; set; }
        public int Status { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public int ActionTime { get; set; }
        public int Locations_id { get; set; }
        public int User_id { get; set; }
        public string Entities { get; set; }
        public string NameUser { get; set; }
        [Required]
        public int GLPI { get; set; }



        public TicketGLPIResponse(int id, int entities_id, int user_agenda_id, string name, DateTime? date,
            DateTime? solveDate, int status, string content, int type, int actionTime, int locations_id,
            int user_id, string entities, string nameUser, int gLPI)
        {
            Id = id;
            Entities_id = entities_id;
            User_agenda_id = user_agenda_id;
            Name = name;
            Date = date;
            SolveDate = solveDate;
            Status = status;
            Content = content;
            Type = type;
            ActionTime = actionTime;
            Locations_id = locations_id;
            User_id = user_id;
            Entities = entities;
            NameUser = nameUser;
            GLPI = gLPI;
        }

        public TicketGLPIResponse()
        {
            Name = string.Empty;
            Content = string.Empty;
            Entities = string.Empty;
            NameUser = string.Empty;
        }
    }
}
