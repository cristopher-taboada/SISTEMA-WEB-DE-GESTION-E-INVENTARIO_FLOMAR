namespace FLOMAR.Models
{
    public class CompraCreateViewModel
    {
        public string numero_compra { get; set; } = string.Empty;
        public DateTime fecha_ingreso { get; set; } = DateTime.Now;
        public int id_proveedor { get; set; }
        public string observaciones { get; set; } = string.Empty;
    }
}
