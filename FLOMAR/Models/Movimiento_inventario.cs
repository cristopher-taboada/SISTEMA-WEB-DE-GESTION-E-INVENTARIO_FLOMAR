﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOMAR.Models
{
    [Table("MOVIMIENTO_INVENTARIO")]
    public class Movimiento_inventario
    {
        [Key]
        [Column("id_movimiento")]
        public int IdMovimiento { get; set; }

        [Column("fecha_ingreso")]
        public DateTime FechaIngreso { get; set; }

        [Column("id_repuesto")]
        public int IdRepuesto { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Column("id_tipo_movimiento")]
        public int IdTipoMovimiento { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Column("observaciones")]
        public string? Observaciones { get; set; }

        [ForeignKey(nameof(IdRepuesto))]
        public Repuesto? Repuesto { get; set; }

        [ForeignKey(nameof(IdTipoMovimiento))]
        public Tipo_movimiento? TipoMovimiento { get; set; }

        [ForeignKey(nameof(IdUsuario))]
        public Usuario? Usuario { get; set; }
    }
}
