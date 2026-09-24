namespace FlomarAPI.Models
{
    // Tabla IMPUESTO: de aqui se toma el porcentaje de IVA vigente (13%)
    public class Impuesto
    {
        public int id_impuesto { get; set; }
        public string nombre_impuesto { get; set; } = string.Empty;
        public decimal porcentaje { get; set; }
        public bool vigente { get; set; }
    }

    // Tabla METODO_PAGO: Efectivo, QR, Tarjeta
    public class MetodoPago
    {
        public int id_metodo_pago { get; set; }
        public string nombre_metodo { get; set; } = string.Empty;
        public string? descripcion { get; set; }
    }
}
