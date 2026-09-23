namespace FlomarAPI.Models.DTOs
{
    // Datos que se muestran en el listado de usuarios (administrador).
    // IMPORTANTE: nunca incluir Contraseña_hash, es informacion sensible
    // que no debe salir de la base de datos.
    public class UsuarioListaDto
    {
        public int id_usuario { get; set; }
        public string Nombre_Usuario { get; set; } = string.Empty;
        public string nombre_completo { get; set; } = string.Empty;
        public int id_rol { get; set; }
        public int id_estado_usuario { get; set; }
        public DateTime fecha_creacion { get; set; }
        public DateTime? ultimo_acceso { get; set; }
    }
}
