namespace agendaback.Model.Request
{
    public class PasswordModel(string password)
    {
        public string Password { get; set; } = password;
    }
}
