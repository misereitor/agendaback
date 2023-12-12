using agendaback.Model.GLPITI;
using Microsoft.EntityFrameworkCore;

namespace agendaback.Data
{
    public class ContextDBGLPITI(DbContextOptions<ContextDBGLPITI> options) : DbContext(options)
    {
        public DbSet<TicketGLPITI> TicketGLPITI { get; set; }
        public DbSet<TicketsUserGLPITI> TicketsUserGLPITI { get; set; }
        public DbSet<UserGLPITI> UserGLPITI { get; set; }
        public DbSet<EntitiesGLPITI> EntitiesGLPITI { get; set; }
    }
}
