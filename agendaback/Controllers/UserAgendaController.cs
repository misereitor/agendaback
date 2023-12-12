using agendaback.ErrorHandling;
using agendaback.Model.Request;
using agendaback.Model.Response;
using agendaback.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace agendaback.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAgendaController(IUserAgendaRepository userAgendaRepository) : ControllerBase
    {
        private readonly IUserAgendaRepository _userAgendaRepository = userAgendaRepository;

        [HttpGet("getuser/{id}")]
        [Authorize]
        public async Task<ActionResult<UserResponse>> GetUserAgendaById(int id)
        {
            UserResponse user = await _userAgendaRepository.GetUserAgendaById(id);
            if (User.IsInRole("Administrator") || (User.Identity != null && user.UserName == User.Identity.Name))
            {
                return Ok(user);
            }
            throw new ErrosException(400, "Usuario não tem permissão para realizar essa ação!");
        }

        [HttpGet("getallusers")]
        [Authorize(Roles = "Administrator, Coordinator, Supervisor")]
        public async Task<ActionResult<List<UserResponse>>> GetAllUserAgenda()
        {
            List<UserResponse> usersAgenda = await _userAgendaRepository.GetAllUserAgenda();
            return Ok(usersAgenda);
        }

        [HttpPut("edituser/{id}")]
        [Authorize]
        public async Task<ActionResult<UserResponse>> EditUserAgenda(int id, UserEdit userAgenda)
        {
            UserResponse user = await _userAgendaRepository.GetUserAgendaById(id);
            if (User.IsInRole("Administrator") || (User.Identity != null && user.UserName == User.Identity.Name))
            {
                UserResponse userEdit = await _userAgendaRepository.EditUserAgenda(id, userAgenda);
                return Ok(userEdit);
            }
            throw new ErrosException(400, "Usuario não tem permissão para realizar essa ação!");
        }

        [HttpPut("alterpassword/{id}")]
        [Authorize]
        public async Task<ActionResult<bool>> AlterPassword(int id, PasswordModel password)
        {
            UserResponse user = await _userAgendaRepository.GetUserAgendaById(id);
            if (User.IsInRole("Administrator") || (User.Identity != null && user.UserName == User.Identity.Name))
            {
                bool userEdit = await _userAgendaRepository.AlterPassword(id, password);
                return Ok(userEdit);
            }
            throw new ErrosException(401, "Usuario não tem permissão para realizar essa ação!");
        }

        [HttpPut("alterroles/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<bool>> EditRolesUserAgenda(int id, RolesEditUserAgenda roles)
        {
            bool deleted = await _userAgendaRepository.EditRolesUserAgenda(id, roles);
            return Ok(deleted);
        }

        [HttpDelete("deliteuser/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<bool>> DeleteUserAgenda(int id)
        {
            bool deleted = await _userAgendaRepository.DeleteUserAgenda(id);
            return Ok(deleted);
        }
    }
}
