﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLOMAR.Models
{
    [Table("VENTA")]
    public class Venta
    {
        [Key]
        [Column("id_venta")]
        public int Id { get; set; }

        [Required]
        [Column("numero_transaccion")]
        public string NumeroTransaccion { get; set; } = string.Empty;

        [Column("fecha_hora")]
        public DateTime FechaHora { get; set; }

        [Column("id_vendedor")]
        public int IdVendedor { get; set; }

        [Column("id_metodo_pago")]
        public int IdMetodoPago { get; set; }

        [Column("id_impuesto")]
        public int IdImpuesto { get; set; }

        [Column("id_estado_venta")]
        public int IdEstadoVenta { get; set; }

        [ForeignKey(nameof(IdVendedor))]
        public Usuario? Vendedor { get; set; }

        [ForeignKey(nameof(IdMetodoPago))]
        public Metodo_pago? MetodoPago { get; set; }

        [ForeignKey(nameof(IdImpuesto))]
        public Impuesto? Impuesto { get; set; }

        [ForeignKey(nameof(IdEstadoVenta))]
        public Estado_venta? EstadoVenta { get; set; }

        public ICollection<detalle_venta> Detalles { get; set; }
            = new List<detalle_venta>();
    }
}
