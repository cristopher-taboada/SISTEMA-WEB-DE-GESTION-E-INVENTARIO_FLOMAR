namespace FLOMAR.Models
{
    public class Impuesto
    {
        public int Id_impuesto { get; set; }
        public string nombre_impuesto { get; set; } = string.Empty;
        public decimal porcentaje { get; set; }
        public int vigente { get; set; }
    }
}
