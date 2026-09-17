namespace FLOMAR.Models
{
    public class Movimiento_inventario
    {
        public int Id_movimiento { get; set; }
        public DateTime Fecha_movimiento { get; set; }
        public int Id_repuesto { get; set; }
        public int Cantidad { get; set; }
        public string id_tipo_movimiento { get; set; } = string.Empty;
        public int Id_usuario { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public string motivo { get; set; } = string.Empty;
    }
}
