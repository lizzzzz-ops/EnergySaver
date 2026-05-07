using Microsoft.EntityFrameworkCore;
using Login_EnergySaver.Models;

namespace Login_EnergySaver.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}