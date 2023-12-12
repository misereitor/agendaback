namespace agendaback.Model.Request
{
    public class UserLogin(string user, string password)
    {
        public string User { get; set; } = user;
        public string Password { get; set; } = password;
    }
}
