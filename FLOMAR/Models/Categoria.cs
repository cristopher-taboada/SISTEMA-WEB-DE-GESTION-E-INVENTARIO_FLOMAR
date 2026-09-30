namespace FLOMAR.Models
{
    public class Categoria
    {
        public int id_categoria { get; set; }

        public string nombre_categoria { get; set; } = string.Empty;

        public string? descripcion { get; set; }
    }
}