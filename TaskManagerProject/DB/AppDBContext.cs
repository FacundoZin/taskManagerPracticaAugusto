using System.Collections.Generic;
using TaskManagerProject.Modelos;

namespace TaskManagerProject.DB
{
    public class AppDBContext : DbContext
    {
        

        public AppDBContext(DbContextOptions<AppContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Tarea> Tareas { get; set; }

        internal void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
