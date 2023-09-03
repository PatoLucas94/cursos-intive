using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spv.Usuarios.Domain.Entities
{
    [Table("DynamicImages")]
    public class DynamicImages
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Nombre { get; set; }

        [Column(TypeName = "image")]
        public byte[] Imagen { get; set; }
    }
}
