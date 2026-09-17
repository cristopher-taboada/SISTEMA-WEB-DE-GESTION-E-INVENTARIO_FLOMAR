namespace FLOMAR.Models
{
    public class Proveedor
    {
        public int Id_proveedor { get; set; }
        public string nombre { get; set; } = string.Empty;
        public int nit { get; set; }
        public string telefono { get; set; } = string.Empty;
        public string direccion { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public int id_estado { get; set; }
    }
}
