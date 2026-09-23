namespace FlomarAPI.Models.DTOs
{
    // Respuesta generica de la API: mensaje legible + campo opcional
    // para que el cliente MVC pueda marcar el input correcto del formulario
    public class RespuestaApi
    {
        public string mensaje { get; set; } = string.Empty;
        public string campo { get; set; } = string.Empty;
    }
}
