using agendaback.AuthSetting;
using agendaback.Data;
using agendaback.ErrorHandling;
using agendaback.Model.Agenda;
using agendaback.Model.GLPITI;
using agendaback.Model.Request;
using agendaback.Model.Response;
using agendaback.Repository.Interface;
using agendaback.Util;
using agendaback.Validations;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace agendaback.Repository
{
    public class AuthRepository(ContextDBAgenda contextDBAgenda, UserValidator userValidator) : IAuthRepository
    {
        private readonly ContextDBAgenda _contextDBAgenda = contextDBAgenda;
        //private readonly ContextDBGLPITI _contextGLPITI = contextGLPITI;
        private readonly UserValidator _userValidator = userValidator;

        public async Task CheckLogin(int userId)
        {
            UserAgenda? user = await _contextDBAgenda.UserAgenda.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null || !user.Active)
            {
                throw new ErrosException(401, "Usuário não tem permissão");
            }
            return;
        }

        public async Task<UserAgenda> CreateUser(UserAgenda user)
        {
            var validationResult = await _userValidator.ValidateAsync(user);
            string[] username = user.UserName.Split(".");
            if (!validationResult.IsValid)
            {
                string errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ErrosException(409, errors);
            }
            //UserGLPITI userGLPITI = await _contextGLPITI.UserGLPITI.FirstOrDefaultAsync(u => u.Name == user.UserName) ?? throw new ErrosException(409, "Usuario não encontrado no GLPI");
            string cripyPassword;
            byte[] bytesPassword = Encoding.UTF8.GetBytes(user.Password);
            byte[] hashBytetes = SHA256.HashData(bytesPassword);

            StringBuilder builder = new();
            for (int i = 0; i < hashBytetes.Length; i++)
            {
                builder.Append(hashBytetes[i].ToString("x2"));
            }
            cripyPassword = builder.ToString();
            user.UserName = user.UserName.ToLower();
            user.Password = cripyPassword;
            user.LastName = username[0] ?? "";
            user.FirstName = username[1] ?? "";
            user.Roles = "Technician";
            user.Active = false;

            try
            {
                await _contextDBAgenda.AddAsync(user);
                await _contextDBAgenda.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }
            user.Password = "";
            return user;
        }

        public async Task<LoginResponse> Login(UserLogin login)
        {
            UserAgenda? user = new();
            TokenAuth token = new();
            LoginResponse loginResponse;
            string passwordCript;
            byte[] bytesPassword = Encoding.UTF8.GetBytes(login.Password); // Converte a senha fornecida em um array de bytes
            byte[] hashBytes = SHA256.HashData(bytesPassword); // Calcula o hash da senha fornecida

            // Converte o hash em uma string hexadecimal
            StringBuilder builder = new();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                builder.Append(hashBytes[i].ToString("x2"));
            }
            passwordCript = builder.ToString();
            try
            {
                user = await _contextDBAgenda.UserAgenda.FirstOrDefaultAsync(u => u.UserName == login.User && u.Password == passwordCript);
            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }
            if (user == null)
            {
                throw new ErrosException(404, "Login e/ou senha invalidas");
            }
            if (!user.Active) throw new ErrosException(403, "Usuário está desativado, consulte o Administrador.");
            token.Token = TokenService.GenerateToken(user);
            loginResponse = ObjectMapper.Map<UserAgenda, LoginResponse>(user);
            loginResponse.Token = token.Token;
            return loginResponse;
        }
    }
}
