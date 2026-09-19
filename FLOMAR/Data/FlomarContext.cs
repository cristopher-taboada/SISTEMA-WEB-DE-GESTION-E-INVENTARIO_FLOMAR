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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Repuesto>().HasKey(r => r.id_repuesto);
            modelBuilder.Entity<Compra>().HasKey(c => c.Id_compra);
            modelBuilder.Entity<Detalle_compra>().HasKey(d => d.Id_detalle_compra);
            modelBuilder.Entity<Proveedor>().HasKey(p => p.Id_proveedor);
            modelBuilder.Entity<Usuario>().HasKey(u => u.id_usuario);
        }
    }
}
