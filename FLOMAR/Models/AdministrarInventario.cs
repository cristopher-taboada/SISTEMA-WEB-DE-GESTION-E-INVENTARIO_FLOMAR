namespace FLOMAR.Models
{
    public class AdministrarInventario
    {
        public int Id_compra { get; set; }
        public string numero_compra { get; set; } = string.Empty;
        public DateTime fecha_ingreso { get; set; }
        public string proveedor { get; set; } = string.Empty;
        public int productos { get; set; }
        public decimal monto_total { get; set; }
        public string registrado_por { get; set; } = string.Empty;
        public string observaciones { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
    }
}
