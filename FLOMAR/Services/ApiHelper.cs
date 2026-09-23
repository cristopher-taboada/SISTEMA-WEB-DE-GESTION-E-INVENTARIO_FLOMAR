using System.Net.Http.Json;
using System.Text.Json;

namespace FLOMAR.Services
{
    // Lee el cuerpo JSON de las respuestas de la FlomarAPI.
    // La API responde con { "mensaje": "...", "campo": "..." } tanto en
    // exito como en error, y con ProblemDetails ({ "title": "..." }) cuando
    // la validacion automatica de [ApiController] rechaza la peticion.
    public static class ApiHelper
    {
        public static async Task<(string mensaje, string campo)> LeerRespuestaAsync(HttpResponseMessage response)
        {
            string mensaje = response.IsSuccessStatusCode
                ? "Operación realizada correctamente."
                : $"Error al comunicar con la API ({(int)response.StatusCode}).";
            string campo = string.Empty;

            try
            {
                var json = await response.Content.ReadFromJsonAsync<JsonElement>();

                if (json.ValueKind == JsonValueKind.Object)
                {
                    if (json.TryGetProperty("mensaje", out var m) && m.ValueKind == JsonValueKind.String)
                        mensaje = m.GetString() ?? mensaje;
                    else if (json.TryGetProperty("title", out var t) && t.ValueKind == JsonValueKind.String)
                        mensaje = t.GetString() ?? mensaje;

                    if (json.TryGetProperty("campo", out var c) && c.ValueKind == JsonValueKind.String)
                        campo = c.GetString() ?? string.Empty;
                }
            }
            catch (JsonException)
            {
                // El cuerpo no era JSON: se mantiene el mensaje por defecto
            }

            return (mensaje, campo);
        }
    }
}
