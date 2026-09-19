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
    }
}
