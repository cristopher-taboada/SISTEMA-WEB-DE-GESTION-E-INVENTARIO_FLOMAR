using System.ComponentModel.DataAnnotations.Schema;

namespace FLOMAR.Models
{
    public class Compra
    {
        public int Id_compra { get; set; }
        public string numero_compra { get; set; } = string.Empty;
        public DateTime fecha_ingreso { get; set; }
        public int id_proveedor { get; set; }
        public int id_usuario { get; set; }
        public decimal monto_total { get; set; }
        public string observaciones { get; set; } = string.Empty;

        [ForeignKey("id_proveedor")]
        public virtual Proveedor? ProveedorRel { get; set; }

        [ForeignKey("id_usuario")]
        public virtual Usuario? UsuarioRel { get; set; }
    }
}
