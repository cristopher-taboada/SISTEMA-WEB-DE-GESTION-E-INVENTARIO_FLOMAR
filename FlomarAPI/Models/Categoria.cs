using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlomarAPI.Models
{
    [Table("CATEGORIA")]
    public class Categoria
    {
        [Key]
        [Column("id_categoria")]
        public int id_categoria { get; set; }

        [Required]
        [StringLength(60)]
        [Column("nombre_categoria")]
        public string nombre_categoria { get; set; } = string.Empty;

        [StringLength(150)]
        [Column("descripcion")]
        public string? descripcion { get; set; }
    }
}