namespace FlomarAPI.Models
{
    public class TipoMovimiento
    {
        public int id_tipo_movimiento { get; set; }
        public string nombre_tipo { get; set; } = string.Empty;
        public string descripcion { get; set; } = string.Empty;
        public int signo { get; set; }
    }
}
