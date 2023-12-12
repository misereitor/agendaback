using agendaback.Data;
using agendaback.ErrorHandling;
using agendaback.Model.Agenda;
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

    public class UserAgendaRepository(ContextDBAgenda contextDBAgenda,
        PasswordValidator validationRules) : IUserAgendaRepository
    {
        private readonly ContextDBAgenda _contextDBAgenda = contextDBAgenda;
        private readonly PasswordValidator _passwordValidator = validationRules;

        public async Task<bool> AlterPassword(int id, PasswordModel password)
        {
            UserAgenda userAgenda = await GetUserAgenda(id);
            var validationResult = await _passwordValidator.ValidateAsync(password);
            if (!validationResult.IsValid)
            {
                string errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ErrosException(409, errors);
            }
            string cripyPassword;
            byte[] bytesPassword = Encoding.UTF8.GetBytes(password.Password);
            byte[] hashBytetes = SHA256.HashData(bytesPassword);

            StringBuilder builder = new();
            for (int i = 0; i < hashBytetes.Length; i++)
            {
                builder.Append(hashBytetes[i].ToString("x2"));
            }
            cripyPassword = builder.ToString();
            userAgenda.Password = cripyPassword;
            try
            {
                _contextDBAgenda.UserAgenda.Update(userAgenda);
                await _contextDBAgenda.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }
            return true;
        }

        public async Task<bool> DeleteUserAgenda(int id)
        {
            UserAgenda userAgenda = await GetUserAgenda(id);
            try
            {
                _contextDBAgenda.UserAgenda.Remove(userAgenda);
                await _contextDBAgenda.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }
            return true;
        }

        public async Task<bool> EditRolesUserAgenda(int id, RolesEditUserAgenda roles)
        {
            UserAgenda userAgenda = await GetUserAgenda(id);
            try
            {
                _contextDBAgenda.UserAgenda.Entry(userAgenda).CurrentValues.SetValues(roles);
                await _contextDBAgenda.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }
            return true;
        }

        public async Task<UserResponse> EditUserAgenda(int id, UserEdit user)
        {
            UserAgenda userAgenda = await GetUserAgenda(id);
            UserResponse usersResponse;
            if (!string.IsNullOrEmpty(user.FirstName))
            {
                userAgenda.FirstName = user.FirstName;
            }

            if (!string.IsNullOrEmpty(user.LastName))
            {
                userAgenda.LastName = user.LastName;
            }
            if (!string.IsNullOrEmpty(user.Email))
            {
                userAgenda.Email = user.Email;
            }
            try
            {
                _contextDBAgenda.UserAgenda.Update(userAgenda);
                await _contextDBAgenda.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }
            usersResponse = ObjectMapper.Map<UserAgenda, UserResponse>(userAgenda);
            return usersResponse;
        }

        public async Task<List<UserResponse>> GetAllUserAgenda()
        {
            List<UserAgenda> usersAgenda;
            List<UserResponse> usersResponse;
            try
            {
                usersAgenda = await _contextDBAgenda.UserAgenda.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }
            usersResponse = ObjectMapper.MapList<UserAgenda, UserResponse>(usersAgenda);
            Console.WriteLine(usersResponse);
            return usersResponse;
        }

        public async Task<UserResponse> GetUserAgendaById(int id)
        {
            UserAgenda? user = new();
            UserResponse userResponse = new();
            try
            {
                user = await _contextDBAgenda.UserAgenda.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }
            if (user == null) throw new ErrosException(404, "Usuário não encontrado!");
            userResponse = ObjectMapper.Map<UserAgenda, UserResponse>(user);
            return userResponse;
        }

        private async Task<UserAgenda> GetUserAgenda(int id)
        {
            UserAgenda? user = new();
            try
            {
                user = await _contextDBAgenda.UserAgenda.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new ErrosException(500, ex.Message);
            }
            if (user == null) throw new ErrosException(404, "Usuário não encontrado!");
            return user;
        }
    }
    
}
