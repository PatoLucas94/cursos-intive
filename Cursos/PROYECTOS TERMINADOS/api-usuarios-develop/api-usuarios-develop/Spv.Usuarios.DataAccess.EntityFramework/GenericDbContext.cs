using Microsoft.EntityFrameworkCore;
using Spv.Usuarios.Domain.Entities;

namespace Spv.Usuarios.DataAccess.EntityFramework
{
    public class GenericDbContext : DbContext
    {
        public GenericDbContext(DbContextOptions<GenericDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<UsuarioRegistrado> UsuarioRegistrado { get; set; }
        public DbSet<DatosUsuario> DatosUsuario { get; set; }
        public DbSet<Configuracion> Configuracion { get; set; }
        public DbSet<AuditoriaLog> Auditoria { get; set; }
        public DbSet<HistorialClaveUsuarios> HistorialClaveUsuarios { get; set; }
        public DbSet<FechaDbServer> FechaDbServer { get; set; }
        public DbSet<DynamicImages> DynamicImages { get; set; }
        public DbSet<DynamicImagesLogin> DynamicImagesLogin { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FechaDbServer>().HasNoKey();
            modelBuilder.Entity<UsuarioRegistrado>().HasKey(ur => new { ur.DocumentTypeId, ur.DocumentNumber });
        }
    }
}