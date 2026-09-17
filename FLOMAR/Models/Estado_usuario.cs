﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOMAR.Models
{
    [Table("ESTADO_USUARIO")]
    public class Estado_usuario
    {
        [Key]
        [Column("id_estado_usuario")]
        public int IdEstadoUsuario { get; set; }

        [Required]
        [Column("nombre_estado")]
        public string NombreEstado { get; set; } = string.Empty;
    }
}
