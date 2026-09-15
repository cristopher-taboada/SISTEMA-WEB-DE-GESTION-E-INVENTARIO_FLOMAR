namespace FLOMAR.Models
{
    public class Movimiento_inventario
    {
        public int Id_inventario { get; set; }
        public DateTime Fecha_ingreso { get; set; }
        public int Id_repuesto { get; set; }
        public int Cantidad { get; set; }
        public string Tipo_movimiento { get; set; } = string.Empty;
        public int Id_usuario { get; set; }
        public string Observaciones { get; set; } = string.Empty;
    }
}
