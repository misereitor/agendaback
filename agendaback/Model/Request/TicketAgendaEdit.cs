namespace agendaback.Model.Request
{
    public class TicketAgendaEdit
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public TicketAgendaEdit(int id, DateTime start, DateTime end)
        {
            Id = id;
            Start = start;
            End = end;
        }

        public TicketAgendaEdit()
        {
        }
    }
}
