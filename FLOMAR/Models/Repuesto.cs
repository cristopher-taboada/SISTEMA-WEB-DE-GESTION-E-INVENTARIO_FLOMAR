using System.ComponentModel.DataAnnotations;

namespace FLOMAR.Models
{
    public class Repuesto
    {
        public int Id { get; set; }

        public string Codigo { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;
        public int Id_categoria { get; set; }
        public decimal costo_adquisicion { get; set; }
        public decimal precio_venta { get; set; }
        public int stock_actual { get; set; }
        public int stock_minimo { get; set; }
        public int id_estado_respuesto { get; set; }
    }
}
