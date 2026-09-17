﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOMAR.Models
{
    [Table("IMPUESTO")]
    public class Impuesto
    {
        [Key]
        [Column("id_impuesto")]
        public int IdImpuesto { get; set; }

        [Required]
        [Column("nombre_impuesto")]
        public string NombreImpuesto { get; set; } = string.Empty;

        [Column("porcentaje", TypeName = "decimal(5,2)")]
        public decimal Porcentaje { get; set; }

        [Column("vigente")]
        public bool Vigente { get; set; }
    }
}
