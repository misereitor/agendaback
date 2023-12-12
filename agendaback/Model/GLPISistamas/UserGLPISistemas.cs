using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agendaback.Model.GLPISistamas
{
    [Table("glpi_users")]
    public class UserGLPISistemas
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Realname { get; set; }
        public string? Firstname { get; set; }
        public bool Is_active { get; set; }

        public UserGLPISistemas(int id, string name, string? realname, string? firstname, bool is_active)
        {
            Id = id;
            Name = name;
            Realname = realname;
            Firstname = firstname;
            Is_active = is_active;
        }

        public UserGLPISistemas()
        {
            Name = string.Empty;
            Realname = string.Empty;
            Firstname = string.Empty;
        }
    }
}
