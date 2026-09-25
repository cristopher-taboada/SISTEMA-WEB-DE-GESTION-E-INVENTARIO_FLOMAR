using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlomarAPI.Models
{
    public class Venta
    {
        [Key]
        public int id_venta { get; set; }

        public int Id_vendedor { get; set; }

        public int id_metodo_pago { get; set; }

        public DateTime Fecha_hora { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal total_venta { get; set; }
    }
}