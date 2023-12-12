namespace agendaback.Model.Request
{
    public class UserEdit
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public UserEdit(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public UserEdit()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
        }
    }
}
