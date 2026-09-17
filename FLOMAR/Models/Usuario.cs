﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOMAR.Models
{
    [Table("TIPO_MOVIMIENTO")]
    public class Tipo_movimiento
    {
        [Key]
        [Column("id_tipo_movimiento")]
        public int IdTipoMovimiento { get; set; }

        [Required]
        [Column("nombre_tipo")]
        public string NombreTipo { get; set; } = string.Empty;

        [Column("descripcion")]
        public string? Descripcion { get; set; }
    }
}
