using Microsoft.EntityFrameworkCore;
using EnergySaver.Models;

namespace EnergySaver.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Dispositivo> Dispositivos { get; set; }

        public DbSet<Configuracion> Configuracion { get; set; }

        public DbSet<Consumo> Consumos { get; set; }  // ← AGREGA
        public DbSet<Consumo> Consumo { get; set; }
    }
}