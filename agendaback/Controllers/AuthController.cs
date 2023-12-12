using agendaback.Model.Agenda;
using agendaback.Model.Request;
using agendaback.Model.Response;
using agendaback.Repository;
using agendaback.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace agendaback.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthRepository authRepository) : ControllerBase
    {
        private readonly IAuthRepository _authRepository = authRepository;

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<UserAgenda>> Register([FromBody] UserAgenda user)
        {
            UserAgenda newUser = await _authRepository.CreateUser(user);
            return Ok(newUser);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] UserLogin user)
        {
            LoginResponse login = await _authRepository.Login(user);
            return Ok(login);
        }

        [HttpGet("checklogin")]
        [Authorize]
        public async Task<ActionResult> CheckLogin()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.Locality);
            if (userIdClaim != null)
            {
                int userId = Int32.Parse(userIdClaim.Value);
                await _authRepository.CheckLogin(userId);
            }
            return Ok();
        }
    }
}
