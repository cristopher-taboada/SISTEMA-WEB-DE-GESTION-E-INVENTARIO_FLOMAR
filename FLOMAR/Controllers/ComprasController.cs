using Microsoft.AspNetCore.Mvc;
using FLOMAR.Models;
using System.Collections.Generic;
using System.Linq;

namespace FLOMAR.Controllers
{
    public class ComprasController : Controller
    {
        // Lista temporal (después se reemplaza por BD)
        private static List<AdministrarInventario> compras = new List<AdministrarInventario>
        {
            new AdministrarInventario
            {
                Id_compra = 1,
                numero_compra = "CMP-2026-001",
                fecha_ingreso = new DateTime(2026, 9, 10, 14, 30, 0),
                proveedor = "Repuestos del Sur S.R.L.",
                productos = 5,
                monto_total = 2450.00m,
                registrado_por = "Carlos Flores Martinez",
                Activo = true
            },
            new AdministrarInventario
            {
                Id_compra = 2,
                numero_compra = "CMP-2026-002",
                fecha_ingreso = new DateTime(2026, 9, 15, 9, 15, 0),
                proveedor = "Importadora Andina Ltda.",
                productos = 3,
                monto_total = 1180.50m,
                registrado_por = "Nestor Maraza Acarapi",
                Activo = true
            },
            new AdministrarInventario
            {
                Id_compra = 3,
                numero_compra = "CMP-2026-003",
                fecha_ingreso = new DateTime(2026, 9, 17, 16, 45, 0),
                proveedor = "Repuestos del Sur S.R.L.",
                productos = 8,
                monto_total = 5320.75m,
                registrado_por = "Carlos Flores Martinez",
                Activo = false
            }
        };

        // Detalle temporal de compras
        private static Dictionary<int, List<DetalleCompraItem>> detalles = new Dictionary<int, List<DetalleCompraItem>>
        {
            {
                1, new List<DetalleCompraItem>
                {
                    new DetalleCompraItem { repuesto = "Pastilla de freno", cantidad = 10, costo_unitario = 45.00m, subtotal = 450.00m },
                    new DetalleCompraItem { repuesto = "Filtro de aceite", cantidad = 15, costo_unitario = 25.00m, subtotal = 375.00m },
                    new DetalleCompraItem { repuesto = "Bujía", cantidad = 20, costo_unitario = 15.00m, subtotal = 300.00m }
                }
            },
            {
                2, new List<DetalleCompraItem>
                {
                    new DetalleCompraItem { repuesto = "Amortiguador delantero", cantidad = 4, costo_unitario = 180.00m, subtotal = 720.00m },
                    new DetalleCompraItem { repuesto = "Terminal de dirección", cantidad = 6, costo_unitario = 76.75m, subtotal = 460.50m }
                }
            },
            {
                3, new List<DetalleCompraItem>
                {
                    new DetalleCompraItem { repuesto = "Disco de freno", cantidad = 8, costo_unitario = 220.00m, subtotal = 1760.00m },
                    new DetalleCompraItem { repuesto = "Líquido de frenos", cantidad = 12, costo_unitario = 35.00m, subtotal = 420.00m },
                    new DetalleCompraItem { repuesto = "Correa de distribución", cantidad = 5, costo_unitario = 620.15m, subtotal = 3100.75m }
                }
            }
        };


        // =========================
        // LISTAR COMPRAS
        // =========================
        public IActionResult Index()
        {
            return View(compras);
        }


        // =========================
        // VER DETALLE
        // =========================
        [HttpGet]
        public IActionResult Details(int id)
        {
            var compra = compras.FirstOrDefault(c => c.Id_compra == id);
            if (compra == null) return NotFound();

            var detalle = new CompraDetalleViewModel
            {
                Id_compra = compra.Id_compra,
                numero_compra = compra.numero_compra,
                fecha_ingreso = compra.fecha_ingreso,
                proveedor = compra.proveedor,
                registrado_por = compra.registrado_por,
                monto_total = compra.monto_total,
                observaciones = "",
                Detalles = detalles.ContainsKey(id) ? detalles[id] : new List<DetalleCompraItem>()
            };

            return View(detalle);
        }


        // =========================
        // MOSTRAR FORMULARIO CREAR
        // =========================
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        // =========================
        // GUARDAR NUEVA COMPRA
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CompraCreateViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                int nuevoId = compras.Count > 0 ? compras.Max(c => c.Id_compra) + 1 : 1;

                var nueva = new AdministrarInventario
                {
                    Id_compra = nuevoId,
                    numero_compra = modelo.numero_compra,
                    fecha_ingreso = modelo.fecha_ingreso,
                    proveedor = "Proveedor #" + modelo.id_proveedor,
                    productos = 0,
                    monto_total = 0,
                    registrado_por = "Sistema",
                    Activo = true
                };

                compras.Add(nueva);
                return RedirectToAction(nameof(Index));
            }

            return View(modelo);
        }


        // =========================
        // MOSTRAR FORMULARIO EDITAR
        // =========================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var compra = compras.FirstOrDefault(c => c.Id_compra == id);
            if (compra == null) return NotFound();
            return View(compra);
        }


        // =========================
        // GUARDAR CAMBIOS
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AdministrarInventario compra)
        {
            if (ModelState.IsValid)
            {
                var existente = compras.FirstOrDefault(c => c.Id_compra == compra.Id_compra);
                if (existente == null) return NotFound();

                existente.numero_compra = compra.numero_compra;
                existente.fecha_ingreso = compra.fecha_ingreso;
                existente.proveedor = compra.proveedor;
                existente.productos = compra.productos;
                existente.monto_total = compra.monto_total;
                existente.registrado_por = compra.registrado_por;

                return RedirectToAction(nameof(Index));
            }

            return View(compra);
        }


        // =========================
        // ACTIVAR / DESACTIVAR
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CambiarEstado(int id)
        {
            var compra = compras.FirstOrDefault(c => c.Id_compra == id);
            if (compra == null) return NotFound();

            compra.Activo = !compra.Activo;
            return RedirectToAction(nameof(Index));
        }
    }
}
