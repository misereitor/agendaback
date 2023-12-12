namespace agendaback.Model.Request
{
    public class TokenAuth
    {
        public string Token { get; set; }

        public TokenAuth(string token)
        {
            Token = token;
        }

        public TokenAuth()
        {
            Token = string.Empty;
        }
    }
}
