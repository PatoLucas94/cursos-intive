using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spv.Usuarios.Domain.Entities
{
    [Table("Configuration")]
    public class Configuracion
    {
        [Key] 
        public int ConfigurationId { get; set; }
        
        [Column(TypeName = "varchar(50)")]
        public string Type { get; set; }
        
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }
        
        [Column(TypeName = "varchar(8000)")]
        public string Value { get; set; }
    }
}
