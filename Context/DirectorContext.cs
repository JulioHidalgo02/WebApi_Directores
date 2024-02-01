using Microsoft.EntityFrameworkCore;
using WebApi_Directores.Entitites;

namespace WebApi_Directores.Context
{
    public class DirectorContext: DbContext
    {
        //Creación del contexto de la base de datos con sus instancias
        public DirectorContext(DbContextOptions<DirectorContext> options)
            :base(options)
        {
            
        }
        //instacias de la base de datos
        public DbSet<Director> Directores { get; set; }
    }
}
