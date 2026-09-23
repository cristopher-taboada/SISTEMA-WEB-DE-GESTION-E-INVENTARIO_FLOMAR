namespace FlomarAPI.Models.DTOs
{
    // Fila del listado de compras (coincide con AdministrarInventario del MVC)
    public class CompraListaDto
    {
        public int Id_compra { get; set; }
        public string numero_compra { get; set; } = string.Empty;
        public DateTime fecha_ingreso { get; set; }
        public string proveedor { get; set; } = string.Empty;
        public int productos { get; set; }
        public decimal monto_total { get; set; }
        public string registrado_por { get; set; } = string.Empty;
    }

    // Detalle completo de una compra (cabecera + lineas)
    public class CompraDetalleDto
    {
        public int Id_compra { get; set; }
        public string numero_compra { get; set; } = string.Empty;
        public DateTime fecha_ingreso { get; set; }
        public int id_proveedor { get; set; }
        public string proveedor { get; set; } = string.Empty;
        public string registrado_por { get; set; } = string.Empty;
        public string observaciones { get; set; } = string.Empty;
        public decimal monto_total { get; set; }
        public int productos { get; set; }
        public List<DetalleCompraItemDto> Detalles { get; set; } = new List<DetalleCompraItemDto>();
    }

    public class DetalleCompraItemDto
    {
        public string codigo { get; set; } = string.Empty;
        public string repuesto { get; set; } = string.Empty;
        public int cantidad { get; set; }
        public decimal costo_unitario { get; set; }
        public decimal subtotal { get; set; }
    }

    // Datos para registrar una compra nueva
    public class CompraCrearRequest
    {
        public string numero_compra { get; set; } = string.Empty;
        public DateTime fecha_ingreso { get; set; }
        public int id_proveedor { get; set; }
        public string observaciones { get; set; } = string.Empty;
        public List<CompraLineaRequest> Productos { get; set; } = new List<CompraLineaRequest>();
    }

    public class CompraLineaRequest
    {
        public int id_repuesto { get; set; }
        public int cantidad { get; set; }
        public decimal costo_unitario { get; set; }
    }

    // Datos editables de la cabecera de una compra
    public class CompraEditarRequest
    {
        public string numero_compra { get; set; } = string.Empty;
        public DateTime fecha_ingreso { get; set; }
        public int id_proveedor { get; set; }
        public decimal monto_total { get; set; }
        public string observaciones { get; set; } = string.Empty;
    }
}
