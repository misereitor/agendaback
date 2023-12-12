using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agendaback.Model.GLPISistamas
{
    [Table("glpi_entities")]
    public class EntitiesGLPISistemas
    {
        [Key]
        public int Id { get; set; }
        public int? Entities_id { get; set; }
        public string Name { get; set; }
        public string CompleteName { get; set; }

        public EntitiesGLPISistemas(int id, int? entities_id, string name, string completeName)
        {
            Id = id;
            Entities_id = entities_id;
            Name = name;
            CompleteName = completeName;
        }

        public EntitiesGLPISistemas()
        {
            Name = string.Empty;
            CompleteName = string.Empty;
        }
    }
}
