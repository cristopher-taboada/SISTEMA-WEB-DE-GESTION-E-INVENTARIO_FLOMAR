using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FLOMAR.Models;
using FLOMAR.Services;

namespace FLOMAR.Controllers
{
    // Toda la logica de negocio (transacciones, stock, validaciones)
    // vive en la FlomarAPI. Este controlador solo orquesta las llamadas HTTP.
    public class ComprasController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ComprasController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // Carga el combo de proveedores y el numero correlativo sugerido
        private async Task CargarDatosFormulario(HttpClient client)
        {
            ViewBag.Proveedores = await client
                .GetFromJsonAsync<List<Proveedor>>("api/proveedores");

            var numeroJson = await client
                .GetFromJsonAsync<JsonElement>("api/compras/siguiente-numero");

            ViewData["SiguienteNumero"] = numeroJson.GetProperty("numero").GetString();
        }

        // =========================
        // LISTAR COMPRAS
        // =========================
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("FlomarAPI");

            var compras = await client
                .GetFromJsonAsync<List<AdministrarInventario>>("api/compras");

            return View(compras);
        }

        // =========================
        // VER DETALLE
        // =========================
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var client = _httpClientFactory.CreateClient("FlomarAPI");

            var response = await client.GetAsync($"api/compras/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return NotFound();

            var modelo = await response.Content
                .ReadFromJsonAsync<CompraDetalleViewModel>();

            return View(modelo);
        }

        // =========================
        // FORMULARIO CREAR (GET)
        // =========================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var client = _httpClientFactory.CreateClient("FlomarAPI");

            await CargarDatosFormulario(client);

            var repuestos = await client
                .GetFromJsonAsync<List<Repuesto>>("api/repuestos");

            var modelo = new CompraCreateFormViewModel
            {
                fecha_ingreso = DateTime.Now
            };

            // Llenar la tabla de productos con todos los repuestos
            foreach (var r in repuestos!)
            {
                modelo.Productos.Add(new ProductoFilaViewModel
                {
                    id_repuesto = r.id_repuesto,
                    codigo = r.Codigo,
                    nombre = r.Nombre,
                    costo_unitario = r.costo_adquisicion,
                    cantidad = 0
                });
            }

            return View(modelo);
        }

        // =========================
        // GUARDAR COMPRA (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompraCreateFormViewModel modelo)
        {
            var client = _httpClientFactory.CreateClient("FlomarAPI");

            if (!ModelState.IsValid)
            {
                await CargarDatosFormulario(client);
                return View(modelo);
            }

            // El ViewModel ya coincide con el contrato del endpoint
            // POST api/compras (numero_compra, fecha_ingreso, id_proveedor,
            // observaciones y Productos con id_repuesto/cantidad/costo_unitario)
            var response = await client.PostAsJsonAsync("api/compras", modelo);

            if (response.IsSuccessStatusCode)
            {
                var (mensajeExito, _) = await ApiHelper.LeerRespuestaAsync(response);
                TempData["Exito"] = mensajeExito;
                return RedirectToAction(nameof(Index));
            }

            // Ej: numero de compra duplicado (409) u otro error de negocio
            var (mensaje, campo) = await ApiHelper.LeerRespuestaAsync(response);
            ModelState.AddModelError(campo, mensaje);

            await CargarDatosFormulario(client);
            return View(modelo);
        }

        // =========================
        // FORMULARIO EDITAR (GET)
        // =========================
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var client = _httpClientFactory.CreateClient("FlomarAPI");

            // El detalle de la API trae todos los campos que necesita
            // CompraEditViewModel (id_proveedor, registrado_por, productos...)
            var response = await client.GetAsync($"api/compras/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return NotFound();

            var modelo = await response.Content
                .ReadFromJsonAsync<CompraEditViewModel>();

            ViewBag.Proveedores = await client
                .GetFromJsonAsync<List<Proveedor>>("api/proveedores");

            return View(modelo);
        }

        // =========================
        // GUARDAR EDICION (POST)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CompraEditViewModel modelo)
        {
            if (id != modelo.Id_compra) return NotFound();

            var client = _httpClientFactory.CreateClient("FlomarAPI");

            if (!ModelState.IsValid)
            {
                // Recargar datos de solo lectura y la lista de proveedores
                var detalleResp = await client.GetAsync($"api/compras/{id}");
                if (detalleResp.StatusCode == HttpStatusCode.NotFound) return NotFound();

                var actual = await detalleResp.Content
                    .ReadFromJsonAsync<CompraEditViewModel>();

                modelo.registrado_por = actual!.registrado_por;
                modelo.productos = actual.productos;

                ViewBag.Proveedores = await client
                    .GetFromJsonAsync<List<Proveedor>>("api/proveedores");

                return View(modelo);
            }

            var response = await client.PutAsJsonAsync($"api/compras/{id}", modelo);

            if (response.StatusCode == HttpStatusCode.NotFound) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // ELIMINAR (la API revierte el stock)
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null) return NotFound();

            var client = _httpClientFactory.CreateClient("FlomarAPI");

            var response = await client.DeleteAsync($"api/compras/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound) return NotFound();

            var (mensaje, _) = await ApiHelper.LeerRespuestaAsync(response);

            if (response.IsSuccessStatusCode)
                TempData["Exito"] = mensaje;
            else
                TempData["Error"] = mensaje;

            return RedirectToAction(nameof(Index));
        }
    }
}
