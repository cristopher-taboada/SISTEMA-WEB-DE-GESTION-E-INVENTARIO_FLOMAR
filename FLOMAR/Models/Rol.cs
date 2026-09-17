﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOMAR.Models
{
    [Table("ROL")]
    public class Rol
    {
        [Key]
        [Column("id_rol")]
        public int IdRol { get; set; }

        [Required]
        [Column("nombre_rol")]
        [StringLength(30)]
        public string NombreRol { get; set; } = string.Empty;

        [Column("descripcion")]
        [StringLength(150)]
        public string? Descripcion { get; set; }
    }
}
