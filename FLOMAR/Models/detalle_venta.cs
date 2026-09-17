﻿using System.ComponentModel.DataAnnotations.Schema;

namespace FLOMAR.Models
{
    [Table("DETALLE_VENTA")]
    public class detalle_venta
    {
        [Column("id_venta")]
        public int IdVenta { get; set; }

        [Column("id_repuesto")]
        public int IdRepuesto { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Column("precio_unitario", TypeName = "decimal(10,2)")]
        public decimal PrecioUnitario { get; set; }

        [ForeignKey(nameof(IdVenta))]
        public Venta? Venta { get; set; }

        [ForeignKey(nameof(IdRepuesto))]
        public Repuesto? Repuesto { get; set; }
    }
}
