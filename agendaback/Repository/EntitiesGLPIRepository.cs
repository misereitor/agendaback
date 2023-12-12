using agendaback.Data;
using agendaback.ErrorHandling;
using agendaback.Model.GLPISistamas;
using agendaback.Model.GLPITI;
using agendaback.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace agendaback.Repository
{
    public class EntitiesGLPIRepository(ContextDBGLPITI contextDBGLPITI, ContextDBGLPISistemas contextDBGLPISistemas) : IEntitiesGLPIRepository
    {
        private readonly ContextDBGLPITI _contextDBGLPITI = contextDBGLPITI;
        private readonly ContextDBGLPISistemas _contextDBGLPISistemas = contextDBGLPISistemas;

        public async Task<EntitiesGLPITI> GetEntitiesGLPITIByIdTicket(int idTicket)
        {
            EntitiesGLPITI? entitiesGLPITI;
            try
            {
                entitiesGLPITI = await _contextDBGLPITI.EntitiesGLPITI.FirstOrDefaultAsync(e => e.Id == idTicket);
            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }
            if (entitiesGLPITI == null)
            {
                throw new ErrosException(404, "Entidade não encontrada");
            }
            return entitiesGLPITI;
        }
        public async Task<EntitiesGLPISistemas> GetEntitiesGLPISistemasByIdTicket(int idTicket)
        {
            EntitiesGLPISistemas? entitiesGLPISistemas;
            try
            {
                entitiesGLPISistemas = await _contextDBGLPISistemas.EntitiesGLPISistemas.FirstOrDefaultAsync(e => e.Id == idTicket);
            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }
            if (entitiesGLPISistemas == null)
            {
                throw new ErrosException(404, "Entidade não encontrada");
            }
            return entitiesGLPISistemas;
        }
    }
}
