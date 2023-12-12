using agendaback.Model.Agenda;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace agendaback.AuthSetting
{
    public class TokenService
    {
        public static string GenerateToken(UserAgenda User)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var topkenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, User.UserName),
                    new Claim(ClaimTypes.NameIdentifier, User.FirstName),
                    new Claim(ClaimTypes.Email, User.Email),
                    new Claim(ClaimTypes.Locality, User.Id.ToString()),
                    new Claim(ClaimTypes.Role, User.Roles.ToString())
                }),
                Expires = DateTime.UtcNow.AddYears(1),
                SigningCredentials = new SigningCredentials
                    (
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature
                    )
            };
            var token = tokenHandler.CreateToken(topkenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
