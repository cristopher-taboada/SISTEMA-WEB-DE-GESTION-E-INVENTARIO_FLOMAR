using Microsoft.EntityFrameworkCore;
using FlomarAPI.Models;

namespace FlomarAPI.Data
{
    public class FlomarContext : DbContext
    {
        public FlomarContext(DbContextOptions<FlomarContext> options) : base(options) { }

        public DbSet<Repuesto> Repuestos { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Detalle_compra> Detalle_compras { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<MovimientoInventario> Movimientos { get; set; }
        public DbSet<TipoMovimiento> TiposMovimiento { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Detalle_venta> Detalle_ventas { get; set; }
        public DbSet<Impuesto> Impuestos { get; set; }
        public DbSet<MetodoPago> MetodosPago { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Repuesto>().ToTable("REPUESTO");
            modelBuilder.Entity<Repuesto>().HasKey(r => r.id_repuesto);
            modelBuilder.Entity<Repuesto>().Property(r => r.Codigo).HasColumnName("codigo");
            modelBuilder.Entity<Repuesto>().Property(r => r.Nombre).HasColumnName("nombre");
            modelBuilder.Entity<Repuesto>().Property(r => r.PrecioVenta).HasColumnName("precio_venta");

            modelBuilder.Entity<Compra>().ToTable("COMPRA");
            modelBuilder.Entity<Compra>().HasKey(c => c.Id_compra);

            modelBuilder.Entity<Detalle_compra>().ToTable("DETALLE_COMPRA");
            modelBuilder.Entity<Detalle_compra>().HasKey(d => d.Id_detalle_compra);

            modelBuilder.Entity<Proveedor>().ToTable("PROVEEDOR");
            modelBuilder.Entity<Proveedor>().HasKey(p => p.Id_proveedor);

            modelBuilder.Entity<Usuario>().ToTable("USUARIO");
            modelBuilder.Entity<Usuario>().HasKey(u => u.id_usuario);
            modelBuilder.Entity<Usuario>().Property(u => u.Nombre_Usuario).HasColumnName("nombre_usuario");
            modelBuilder.Entity<Usuario>().Property(u => u.Contraseña_hash).HasColumnName("contrasena_hash");

            modelBuilder.Entity<MovimientoInventario>().ToTable("MOVIMIENTO_INVENTARIO");
            modelBuilder.Entity<MovimientoInventario>().HasKey(m => m.id_movimiento);

            modelBuilder.Entity<TipoMovimiento>().ToTable("TIPO_MOVIMIENTO");
            modelBuilder.Entity<TipoMovimiento>().HasKey(t => t.id_tipo_movimiento);

            modelBuilder.Entity<Venta>().ToTable("VENTA");
            modelBuilder.Entity<Venta>().HasKey(v => v.id_venta);

            modelBuilder.Entity<Detalle_venta>().ToTable("DETALLE_VENTA");
            modelBuilder.Entity<Detalle_venta>().HasKey(d => d.id_detalle_venta);

            modelBuilder.Entity<Impuesto>().ToTable("IMPUESTO");
            modelBuilder.Entity<Impuesto>().HasKey(i => i.id_impuesto);

            modelBuilder.Entity<MetodoPago>().ToTable("METODO_PAGO");
            modelBuilder.Entity<MetodoPago>().HasKey(m => m.id_metodo_pago);
        }
    }
}
