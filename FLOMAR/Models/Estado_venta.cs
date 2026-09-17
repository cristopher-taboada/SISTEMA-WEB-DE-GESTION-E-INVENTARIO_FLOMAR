﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOMAR.Models
{
    [Table("ESTADO_VENTA")]
    public class Estado_venta
    {
        [Key]
        [Column("id_estado_venta")]
        public int IdEstadoVenta { get; set; }

        [Required]
        [Column("nombre_estado")]
        public string NombreEstado { get; set; } = string.Empty;
    }
}
