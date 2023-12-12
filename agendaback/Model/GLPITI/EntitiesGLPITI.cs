using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace agendaback.Model.GLPITI
{
    [Table("glpi_entities")]
    public class EntitiesGLPITI
    {
        [Key]
        public int Id { get; set; }
        public int? Entities_id { get; set; }
        public string Name { get; set; }
        public string CompleteName { get; set; }

        public EntitiesGLPITI(int id, int? entities_id, string name, string completeName)
        {
            Id = id;
            Entities_id = entities_id;
            Name = name;
            CompleteName = completeName;
        }

        public EntitiesGLPITI()
        {
            Name = string.Empty;
            CompleteName = string.Empty;
        }
    }
}
