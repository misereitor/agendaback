using agendaback.Data;
using agendaback.ErrorHandling;
using agendaback.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace agendaback.Repository
{
    public class UserAgendaModelValidatorRepository(ContextDBAgenda contextDBAgenda, ContextDBGLPITI contextGLPITI) : IUserAgendaModelValidatorRepository
    {
        private readonly ContextDBAgenda _contextDBAgenda = contextDBAgenda;
        private readonly ContextDBGLPITI _contextGLPITI = contextGLPITI;

        public async Task<bool> ExclusiveEmail(string email)
        {
            bool exist;
            try
            {
                exist = await _contextDBAgenda.UserAgenda.AnyAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }
            return !exist;
        }

        public async Task<bool> ExclusiveUser(string userName)
        {
            bool exist;
            try
            {
                exist = await _contextDBAgenda.UserAgenda.AnyAsync(u => u.UserName == userName);
            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }
            return !exist;
        }

        public async Task<bool> UserFound(string userName)
        {
            bool exist;
            try
            {
                exist = await _contextGLPITI.UserGLPITI.AnyAsync(u => u.Name == userName);
            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }
            return exist;
        }
    }
}
