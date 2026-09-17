using Microsoft.EntityFrameworkCore;
using FLOMAR.Models;

namespace FLOMAR.Data
{
    public class FlomarContext : DbContext
    {
        public FlomarContext(DbContextOptions<FlomarContext> options)
            : base(options)
        {
        }

        // ==========================================
        // USUARIOS Y ROLES
        // ==========================================

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Estado_usuario> EstadosUsuario { get; set; }


        // ==========================================
        // REPUESTOS
        // ==========================================

        public DbSet<Repuesto> Repuestos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Estado_respuesto> EstadosRepuesto { get; set; }


        // ==========================================
        // VENTAS
        // ==========================================

        public DbSet<Venta> Ventas { get; set; }
        public DbSet<detalle_venta> DetallesVenta { get; set; }
        public DbSet<Estado_venta> EstadosVenta { get; set; }


        // ==========================================
        // PAGOS E IMPUESTOS
        // ==========================================

        public DbSet<Metodo_pago> MetodosPago { get; set; }
        public DbSet<Impuesto> Impuestos { get; set; }


        // ==========================================
        // INVENTARIO
        // ==========================================

        public DbSet<Movimiento_inventario> MovimientosInventario { get; set; }
        public DbSet<Tipo_movimiento> TiposMovimiento { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==========================================
            // CLAVE COMPUESTA DETALLE_VENTA
            // ==========================================

            modelBuilder.Entity<detalle_venta>()
                .HasKey(d => new
                {
                    d.IdVenta,
                    d.IdRepuesto
                });


            // ==========================================
            // RELACIÓN REPUESTO - CATEGORIA
            // ==========================================

            modelBuilder.Entity<Repuesto>()
                .HasOne(r => r.Categoria)
                .WithMany(c => c.Repuestos)
                .HasForeignKey(r => r.IdCategoria);


            // ==========================================
            // RELACIÓN REPUESTO - ESTADO
            // ==========================================

            modelBuilder.Entity<Repuesto>()
                .HasOne(r => r.EstadoRepuesto)
                .WithMany()
                .HasForeignKey(r => r.IdEstadoRepuesto);


            // ==========================================
            // RELACIÓN USUARIO - ROL
            // ==========================================

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Rol)
                .WithMany()
                .HasForeignKey(u => u.IdRol);


            // ==========================================
            // RELACIÓN USUARIO - ESTADO
            // ==========================================

            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.EstadoUsuario)
                .WithMany()
                .HasForeignKey(u => u.IdEstadoUsuario);


            // ==========================================
            // VENTA - VENDEDOR
            // ==========================================

            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Vendedor)
                .WithMany()
                .HasForeignKey(v => v.IdVendedor);


            // ==========================================
            // VENTA - MÉTODO DE PAGO
            // ==========================================

            modelBuilder.Entity<Venta>()
                .HasOne(v => v.MetodoPago)
                .WithMany()
                .HasForeignKey(v => v.IdMetodoPago);


            // ==========================================
            // VENTA - IMPUESTO
            // ==========================================

            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Impuesto)
                .WithMany()
                .HasForeignKey(v => v.IdImpuesto);


            // ==========================================
            // VENTA - ESTADO
            // ==========================================

            modelBuilder.Entity<Venta>()
                .HasOne(v => v.EstadoVenta)
                .WithMany()
                .HasForeignKey(v => v.IdEstadoVenta);


            // ==========================================
            // DETALLE - VENTA
            // ==========================================

            modelBuilder.Entity<detalle_venta>()
                .HasOne(d => d.Venta)
                .WithMany(v => v.Detalles)
                .HasForeignKey(d => d.IdVenta);


            // ==========================================
            // DETALLE - REPUESTO
            // ==========================================

            modelBuilder.Entity<detalle_venta>()
                .HasOne(d => d.Repuesto)
                .WithMany()
                .HasForeignKey(d => d.IdRepuesto);


            // ==========================================
            // MOVIMIENTO - REPUESTO
            // ==========================================

            modelBuilder.Entity<Movimiento_inventario>()
                .HasOne(m => m.Repuesto)
                .WithMany()
                .HasForeignKey(m => m.IdRepuesto);


            // ==========================================
            // MOVIMIENTO - TIPO
            // ==========================================

            modelBuilder.Entity<Movimiento_inventario>()
                .HasOne(m => m.TipoMovimiento)
                .WithMany()
                .HasForeignKey(m => m.IdTipoMovimiento);


            // ==========================================
            // MOVIMIENTO - USUARIO
            // ==========================================

            modelBuilder.Entity<Movimiento_inventario>()
                .HasOne(m => m.Usuario)
                .WithMany()
                .HasForeignKey(m => m.IdUsuario);
        }
    }
}
