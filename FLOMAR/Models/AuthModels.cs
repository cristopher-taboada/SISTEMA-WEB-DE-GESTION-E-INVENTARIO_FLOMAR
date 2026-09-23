namespace FLOMAR.Models
{
    // Respuesta del endpoint POST api/auth/login de la FlomarAPI
    public class LoginResponse
    {
        public int id_usuario { get; set; }
        public string nombre_completo { get; set; } = string.Empty;
        public int id_rol { get; set; }
    }
}
