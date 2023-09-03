using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spv.Usuarios.Domain.Entities
{
    [Table("DynamicImagesLogin")]
    public class DynamicImagesLogin
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Nombre { get; set; }

        public int IdImagen { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string Link { get; set; }

        public int Orden { get; set; }

        public bool Habilitada { get; set; }
    }
}
