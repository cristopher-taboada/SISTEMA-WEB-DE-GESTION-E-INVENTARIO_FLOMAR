﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOMAR.Models
{
    [Table("ESTADO_REPUESTO")]
    public class Estado_respuesto
    {
        [Key]
        [Column("id_estado_repuesto")]
        public int IdEstadoRepuesto { get; set; }

        [Required]
        [Column("nombre_estado")]
        public string NombreEstado { get; set; } = string.Empty;
    }
}
