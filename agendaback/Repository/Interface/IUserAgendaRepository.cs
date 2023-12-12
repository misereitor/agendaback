using agendaback.Model.Request;
using agendaback.Model.Response;

namespace agendaback.Repository.Interface
{
    public interface IUserAgendaRepository
    {
        Task<UserResponse> GetUserAgendaById(int id);
        Task<List<UserResponse>> GetAllUserAgenda();
        Task<UserResponse> EditUserAgenda(int id, UserEdit user);
        Task<bool> EditRolesUserAgenda(int id, RolesEditUserAgenda roles);
        Task<bool> AlterPassword(int id, PasswordModel password);
        Task<bool> DeleteUserAgenda(int id);
    }
}
