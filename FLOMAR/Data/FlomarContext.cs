using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore;
using FLOMAR.Models;

namespace FLOMAR.Data
{
    public class FlomarContext : DbContext
    {
        public FlomarContext(DbContextOptions<FlomarContext> options) : base(options) { }

        public DbSet<Repuesto> Repuestos { get; set; }

        
        public DbSet<Usuario> usuario { get; set; }
    }
}