namespace FlomarAPI.Models
{
    public class Usuario
    {
        public int id_usuario { get; set; }
        public string Nombre_Usuario { get; set; } = string.Empty;
        public string Contraseña_hash { get; set; } = string.Empty;
        public string nombre_completo { get; set; } = string.Empty;

        public int id_rol { get; set; }
        public int id_estado_usuario { get; set; }
        public DateTime fecha_creacion { get; set; }
        public DateTime? ultimo_acceso { get; set; }
    }
}
