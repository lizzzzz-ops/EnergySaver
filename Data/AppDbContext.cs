<<<<<<< HEAD
﻿using Microsoft.EntityFrameworkCore;
using Login_EnergySaver.Models;

namespace Login_EnergySaver.Data
=======
﻿using EnergySaver.Models;
using Microsoft.EntityFrameworkCore;

namespace EnergySaver.Data
>>>>>>> 7065bafb3b2de88c4632b6748e8432d70a0a39bc
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
<<<<<<< HEAD
=======
        public DbSet<Dispositivo> Dispositivos { get; set; }
>>>>>>> 7065bafb3b2de88c4632b6748e8432d70a0a39bc
    }
}