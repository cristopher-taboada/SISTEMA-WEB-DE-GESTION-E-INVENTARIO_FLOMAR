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


        // =========================
        // LISTAR COMPRAS
        // =========================
        public IActionResult Index()
        {
            return View(compras);
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
