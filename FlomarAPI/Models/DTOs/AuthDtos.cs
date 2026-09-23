namespace FlomarAPI.Models.DTOs
{
    public class LoginRequest
    {
        public string usuario { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public int id_usuario { get; set; }
        public string nombre_completo { get; set; } = string.Empty;
        public int id_rol { get; set; }
    }
}
