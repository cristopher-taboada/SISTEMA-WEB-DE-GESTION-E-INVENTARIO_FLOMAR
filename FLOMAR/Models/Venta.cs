namespace FLOMAR.Models
{
    public class Venta
    {
        public int Id { get; set; }
        public string numero_transaccion { get; set; } = string.Empty;
        public DateTime Fecha_hora { get; set; }
        public int Id_vendedor { get; set; }
        public int id_metodo_pago { get; set; }
        public int id_impuesto { get; set; }
        public int id_estado_venta { get; set; }
    }
}
