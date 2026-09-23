namespace FlomarAPI.Models.DTOs
{
    // Fila del kardex (coincide con MovimientoListaViewModel del MVC)
    public class MovimientoListaDto
    {
        public int id_movimiento { get; set; }
        public int id_repuesto { get; set; }
        public DateTime fecha_movimiento { get; set; }
        public string codigo_repuesto { get; set; } = string.Empty;
        public string nombre_repuesto { get; set; } = string.Empty;
        public string tipo_movimiento { get; set; } = string.Empty;
        public int signo { get; set; }
        public int cantidad { get; set; }
        public string motivo { get; set; } = string.Empty;
        public string observaciones { get; set; } = string.Empty;
        public string usuario { get; set; } = string.Empty;
        public int stock_resultante { get; set; }
    }

    // Detalle de un movimiento (coincide con MovimientoDetalleViewModel del MVC)
    public class MovimientoDetalleDto
    {
        public int id_movimiento { get; set; }
        public DateTime fecha_movimiento { get; set; }
        public string codigo_repuesto { get; set; } = string.Empty;
        public string nombre_repuesto { get; set; } = string.Empty;
        public int stock_actual { get; set; }
        public int stock_minimo { get; set; }
        public string tipo_movimiento { get; set; } = string.Empty;
        public int signo { get; set; }
        public int cantidad { get; set; }
        public string motivo { get; set; } = string.Empty;
        public string observaciones { get; set; } = string.Empty;
        public string usuario { get; set; } = string.Empty;
    }

    // Datos para registrar un movimiento (ajuste / merma manual)
    public class MovimientoCrearRequest
    {
        public int id_repuesto { get; set; }
        public int id_tipo_movimiento { get; set; }
        public int cantidad { get; set; }
        public string motivo { get; set; } = string.Empty;
        public string observaciones { get; set; } = string.Empty;
    }
}
