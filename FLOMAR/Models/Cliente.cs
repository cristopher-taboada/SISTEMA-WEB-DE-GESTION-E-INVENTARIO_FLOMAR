namespace FLOMAR.Models
{
    public class Cliente
    {

        public int Id { get; set; }
        public string nombre { get; set; } = string.Empty;
        public string nit { get; set; } = string.Empty;
        public string telefono { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string direccion { get; set; } = string.Empty;
        public DateTime fecha_registro { get; set; }
        public int id_estado { get; set; } 

    }
}
