using agendaback.Model.GLPISistamas;
using Microsoft.EntityFrameworkCore;

namespace agendaback.Data
{
    public class ContextDBGLPISistemas(DbContextOptions<ContextDBGLPISistemas> options) : DbContext(options)
    {
        public DbSet<TicketGLPISistemas> TicketGLPISistemas { get; set; }
        public DbSet<TicketsUserGLPISistemas> TicketsUserGLPISistemas { get; set; }
        public DbSet<UserGLPISistemas> UserGLPISistemas { get; set; }
        public DbSet<EntitiesGLPISistemas> EntitiesGLPISistemas { get; set; }
    }
}
