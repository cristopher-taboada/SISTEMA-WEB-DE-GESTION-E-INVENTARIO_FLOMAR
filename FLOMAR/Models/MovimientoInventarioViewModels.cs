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
}
