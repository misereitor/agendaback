using agendaback.Model.Agenda;
using agendaback.Model.Request;
using agendaback.Model.Response;

namespace agendaback.Repository.Interface
{
    public interface IAuthRepository
    {
        Task CheckLogin(int userId);
        Task<UserAgenda> CreateUser(UserAgenda user);
        Task<LoginResponse> Login(UserLogin login);
    }
}
