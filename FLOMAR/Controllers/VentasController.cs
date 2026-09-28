using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using FLOMAR.Models;
using FLOMAR.Services;

namespace FLOMAR.Controllers
{
    // La logica de la venta (validar stock, calcular IVA, descontar stock,
    // guardar en la BD) vive en la FlomarAPI. Este controlador solo
    // orquesta las llamadas HTTP del punto de venta.
    public class VentasController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public VentasController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // POST: /Ventas/Confirmar
        // Recibe el carrito en JSON (fetch desde Vendedor.cshtml),
        // lo reenvia a la API y devuelve JSON para que el JS redirija.
        [HttpPost]
        public async Task<IActionResult> Confirmar([FromBody] VentaCrearRequest modelo)
        {
            if (modelo.lineas == null || modelo.lineas.Count == 0)
                return BadRequest(new { ok = false, mensaje = "El carrito esta vacio." });

            // El vendedor es el usuario que inicio sesion
            modelo.id_vendedor = LoginController.IdUsuarioActual > 0
                ? LoginController.IdUsuarioActual
                : 1;

            var client = _httpClientFactory.CreateClient("FlomarAPI");

            var response = await client.PostAsJsonAsync("api/ventas", modelo);

            if (response.IsSuccessStatusCode)
            {
                var creada = await response.Content
                    .ReadFromJsonAsync<VentaDetalleViewModel>();

                return Json(new { ok = true, idVenta = creada!.id_venta });
            }

            // Ej: stock insuficiente (409) u otro error de negocio
            var (mensaje, _) = await ApiHelper.LeerRespuestaAsync(response);
            return StatusCode((int)response.StatusCode, new { ok = false, mensaje });
        }

        // GET: /Ventas/Detalle/5  (comprobante de la venta)
        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var client = _httpClientFactory.CreateClient("FlomarAPI");

            var response = await client.GetAsync($"api/ventas/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return NotFound();

            var modelo = await response.Content
                .ReadFromJsonAsync<VentaDetalleViewModel>();

            return View(modelo);
        }
    }
}
