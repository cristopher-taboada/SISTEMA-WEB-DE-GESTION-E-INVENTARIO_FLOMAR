namespace FLOMAR.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre_Usuario { get; set; } = string.Empty;
        public string Contraseña_hash { get; set; } = string.Empty;
        public string nombre_completo { get; set; } = string.Empty;

        public int id_rol { get; set; }
        public int id_estado_usuario { get; set; }
    }
}
