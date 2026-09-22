using System.ComponentModel.DataAnnotations;

namespace FLOMAR.Models
{
    public class AdministrarInventario
    {
        public int Id_compra { get; set; }

        [Required(ErrorMessage = "El número de compra es obligatorio.")]
        [StringLength(20, MinimumLength = 5,
            ErrorMessage = "El número de compra debe tener entre 5 y 20 caracteres.")]
        public string numero_compra { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de ingreso es obligatoria.")]
        public DateTime fecha_ingreso { get; set; }

        public string proveedor { get; set; } = string.Empty;

        public int productos { get; set; }

        [Required(ErrorMessage = "El monto total es obligatorio.")]
        [Range(0.01, 999999, ErrorMessage = "El monto total debe ser mayor a 0.")]
        public decimal monto_total { get; set; }

        public string registrado_por { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Las observaciones no pueden superar los 200 caracteres.")]
        public string observaciones { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;
    }
}
