using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace agendaback.Model.Agenda
{
    [Table("agenda_user")]
    public class UserAgenda
    {
        [Key]
        public int Id { get; set; }
        public int IdGLPITI { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [DefaultValue("Technician")]
        public string Roles { get; set; }
        public byte[]? Picture { get; set; }
        public int IdGLPISistemas { get; set; }
        [DefaultValue(false)]
        public bool Active { get; set; }

        public UserAgenda(int id, int idGLPI, string userName, string firstName, string lastName,
            string password, string email, string roles, byte[] picture, int idGLPISistemas, bool active)
        {
            Id = id;
            IdGLPITI = idGLPI;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Email = email;
            Roles = roles;
            Picture = picture;
            IdGLPISistemas = idGLPISistemas;
            Active = active;
        }

        public UserAgenda()
        {
            UserName = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
            Roles = string.Empty;
        }
    }
}
