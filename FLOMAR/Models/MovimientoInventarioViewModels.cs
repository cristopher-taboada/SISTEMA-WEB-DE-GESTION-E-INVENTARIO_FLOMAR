using System.ComponentModel.DataAnnotations;

namespace FLOMAR.Models
{
    public class MovimientoListaViewModel
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

    public class MovimientoCrearViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar un repuesto.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un repuesto.")]
        public int id_repuesto { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un tipo de movimiento.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de movimiento.")]
        public int id_tipo_movimiento { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, 99999, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int cantidad { get; set; }

        [Required(ErrorMessage = "El motivo es obligatorio.")]
        [StringLength(100)]
        public string motivo { get; set; } = string.Empty;

        [StringLength(200)]
        public string observaciones { get; set; } = string.Empty;
    }

    public class MovimientoDetalleViewModel
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


    // ALERTA DE INMOVILIZADOS: repuesto que no se movio en los ultimos N meses
    public class RepuestoSinMovimientoViewModel
    {
        public int id_repuesto { get; set; }
        public string codigo { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public int stock_actual { get; set; }
        public int stock_minimo { get; set; }
        public DateTime? ultimo_movimiento { get; set; }

        // "Kardex" o "Venta": de donde vino el ultimo movimiento del repuesto
        public string origen_ultimo_movimiento { get; set; } = string.Empty;

        public int? dias_sin_movimiento { get; set; }
        public decimal valor_inmovilizado { get; set; }
    }

    // Modelo de la vista Index del kardex: los movimientos + la alerta
    public class MovimientoIndexViewModel
    {
        public List<MovimientoListaViewModel> Movimientos { get; set; } = new();

        public List<RepuestoSinMovimientoViewModel> SinMovimiento { get; set; } = new();

        // Meses configurados para la alerta (appsettings.json o ?meses=N)
        public int MesesSinMovimiento { get; set; } = 6;
    }
}
