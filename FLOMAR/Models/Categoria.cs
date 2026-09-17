﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOMAR.Models
{
    [Table("CATEGORIA")]
    public class Categoria
    {
        [Key]
        [Column("id_categoria")]
        public int Id { get; set; }

        [Required]
        [Column("nombre_categoria")]
        [StringLength(60)]
        public string NombreCategoria { get; set; } = string.Empty;

        [Column("descripcion")]
        [StringLength(150)]
        public string? Descripcion { get; set; }

        public ICollection<Repuesto> Repuestos { get; set; }
            = new List<Repuesto>();
    }
}
