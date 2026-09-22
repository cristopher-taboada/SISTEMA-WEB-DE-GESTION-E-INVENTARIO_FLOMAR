namespace FLOMAR.Models
{
    public class MovimientoInventario
    {
        public int id_movimiento { get; set; }
        public DateTime fecha_movimiento { get; set; }
        public int id_repuesto { get; set; }
        public int cantidad { get; set; }
        public int id_tipo_movimiento { get; set; }
        public int id_usuario { get; set; }
        public string motivo { get; set; } = string.Empty;
        public string observaciones { get; set; } = string.Empty;
    }
}
