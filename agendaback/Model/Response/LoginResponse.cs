namespace agendaback.Model.Response
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public int IdGLPITI { get; set; }
        public int IdGLPISistemas { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
        public string Token { get; set; }

        public LoginResponse(int id, int idGLPI, int idGLPISistemas, string userName, string firstName, string lastName, string email, string roles, string token)
        {
            Id = id;
            IdGLPITI = idGLPI;
            IdGLPISistemas = idGLPISistemas;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Roles = roles;
            Token = token;
        }

        public LoginResponse()
        {
            UserName = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Roles = string.Empty;
            Token = string.Empty;
        }
    }

}
