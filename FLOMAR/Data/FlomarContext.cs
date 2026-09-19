using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;
using FLOMAR.Models;

namespace FLOMAR.Data
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
        }
    }
}
