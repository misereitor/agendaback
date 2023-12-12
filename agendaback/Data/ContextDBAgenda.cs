using agendaback.Model.Agenda;
using Microsoft.EntityFrameworkCore;

namespace agendaback.Data
{
    public class ContextDBAgenda(DbContextOptions<ContextDBAgenda> options) : DbContext(options)
    {
        public DbSet<UserAgenda> UserAgenda { get; set; }
        public DbSet<TicketAgenda> TicketAgenda { get; set; }
    }
}
