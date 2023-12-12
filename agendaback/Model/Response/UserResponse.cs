using System.ComponentModel;

namespace agendaback.Model.Response
{
    public class UserResponse
    {
        public int Id { get; set; }
        public int IdGLPITI { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [DefaultValue(0)]
        public string Roles { get; set; }
        public int IdGLPISistemas { get; set; }
        public bool Active { get; set; }


        public UserResponse(int id, int idGLPI, string userName, string firstName, string lastName, string email, string roles, int idGLPISistemas, bool active)
        {
            Id = id;
            IdGLPITI = idGLPI;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Roles = roles;
            IdGLPISistemas = idGLPISistemas;
            Active = active;
        }

        public UserResponse()
        {
            UserName = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Roles = string.Empty;
        }
    }
}
