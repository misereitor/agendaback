using agendaback.Model.GLPISistamas;
using agendaback.Model.GLPITI;

namespace agendaback.Repository.Interface
{
    public interface IEntitiesGLPIRepository
    {
        public Task<EntitiesGLPITI> GetEntitiesGLPITIByIdTicket(int idTicket);
        public Task<EntitiesGLPISistemas> GetEntitiesGLPISistemasByIdTicket(int idTicket);
        
    }
}
