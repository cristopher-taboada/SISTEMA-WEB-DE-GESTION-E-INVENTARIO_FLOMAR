﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOMAR.Models
{
    [Table("METODO_PAGO")]
    public class Metodo_pago
    {
        [Key]
        [Column("id_metodo_pago")]
        public int IdMetodoPago { get; set; }

        [Required]
        [Column("nombre_metodo")]
        public string NombreMetodo { get; set; } = string.Empty;

        [Column("descripcion")]
        public string? Descripcion { get; set; }
    }
}
