namespace agendaback.Model.Request
{
    public class RolesEditUserAgenda
    {
        public string Roles { get; set; }

        public RolesEditUserAgenda(string roles)
        {
            Roles = roles;
        }
        public RolesEditUserAgenda()
        {
            Roles = string.Empty;
        }
    }
}
