using Microsoft.EntityFrameworkCore;
using Spv.Usuarios.Domain.Entities;
using Spv.Usuarios.Domain.Entities.V2;

namespace Spv.Usuarios.DataAccess.EntityFramework
{
    public class GenericDbContextV2 : DbContext
    {
        public GenericDbContextV2(DbContextOptions<GenericDbContextV2> options) : base(options)
        {
        }

        public DbSet<UsuarioV2> Usuario { get; set; }
        public DbSet<ConfiguracionV2> Configuracion { get; set; }
        public DbSet<AuditoriaLogV2> Auditoria { get; set; }
        public DbSet<EstadosUsuarioV2> EstadosUsuario { get; set; }
        public DbSet<TiposEventoV2> TiposEvento { get; set; }
        public DbSet<ResultadosEventoV2> ResultadosEvento { get; set; }
        public DbSet<HistorialClaveUsuariosV2> HistorialClaveUsuarios { get; set; }
        public DbSet<HistorialUsuarioUsuariosV2> HistorialUsuarioUsuarios { get; set; }
        public DbSet<FechaDbServerV2> FechaDbServer { get; set; }
        public DbSet<ReglaValidacionV2> ReglaValidacion { get; set; }
        public DbSet<DynamicImages> DynamicImages { get; set; }
        public DbSet<DynamicImagesLogin> DynamicImagesLogin { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FechaDbServerV2>()
                .HasNoKey();

            modelBuilder.Entity<UsuarioV2>(user =>
            {
                user.ToTable("Users");
                user.HasKey(u => u.UserId);
                user.HasIndex(u => new
                {
                    Document_Country_Id = u.DocumentCountryId,
                    Document_Type_Id = u.DocumentTypeId,
                    Document_Number = u.DocumentNumber
                }).IsUnique();
                user.HasMany(s => s.UserPasswordHistory).WithOne(h => h.Usuario).HasForeignKey(u => u.UserId);
                user.HasMany(s => s.UserUsernameHistory).WithOne(h => h.Usuario).HasForeignKey(u => u.UserId);
                user.Property(u => u.LoginAttempts).IsRequired().HasDefaultValue(0);
                user.Property(u => u.CreatedDate).IsRequired().HasDefaultValueSql("GetDate()");
                user.Property(o => o.DocumentNumber).IsRequired().HasColumnType("varchar").HasMaxLength(20);
                user.Property(o => o.Username).IsRequired().HasColumnType("varchar").HasMaxLength(100);
                user.Property(o => o.Password).IsRequired().HasColumnType("varchar").HasMaxLength(100);
            });

            modelBuilder.Entity<ConfiguracionV2>(entity =>
            {
                entity.ToTable("Configurations");
                entity.HasKey(u => u.ConfigurationId);
                entity.Property(o => o.Type).IsRequired().HasColumnType("varchar").HasMaxLength(50);
                entity.Property(o => o.Name).IsRequired().HasColumnType("varchar").HasMaxLength(50);
                entity.Property(o => o.Description).HasColumnType("varchar").HasMaxLength(200);
                entity.Property(o => o.Value).HasColumnType("varchar").HasMaxLength(8000);
                entity.Property(o => o.Rol).HasColumnType("varchar").HasMaxLength(50);
            });

            modelBuilder.Entity<AuditoriaLogV2>(entity =>
            {
                entity.ToTable("AuditLogs");
                entity.HasKey(u => u.AuditLogId);
                entity.Property(o => o.Channel).HasColumnType("varchar").HasMaxLength(10);
            });

            modelBuilder.Entity<EstadosUsuarioV2>(entity =>
            {
                entity.ToTable("UserStatuses");
                entity.HasKey(u => u.UserStatusId);
                entity.HasMany(s => s.Users).WithOne(u => u.Status).HasForeignKey(f => f.UserStatusId);
                entity.Property(o => o.Description).IsRequired().HasColumnType("varchar").HasMaxLength(20);
            });

            modelBuilder.Entity<TiposEventoV2>(entity =>
            {
                entity.ToTable("EventTypes");
                entity.HasKey(u => u.EventTypeId);
                entity.HasMany(a => a.Audits).WithOne(a => a.EventTypes).HasForeignKey(a => a.EventTypeId);
                entity.Property(o => o.Name).HasColumnType("varchar").HasMaxLength(50);
                entity.Property(o => o.Description).HasColumnType("varchar").HasMaxLength(100);
            });

            modelBuilder.Entity<ResultadosEventoV2>(entity =>
            {
                entity.ToTable("EventResults");
                entity.HasKey(u => u.EventResultId);
                entity.HasMany(e => e.Audits).WithOne(a => a.EventResults).HasForeignKey(a => a.EventResultId);
                entity.Property(o => o.Description).IsRequired().HasColumnType("varchar").HasMaxLength(100);
            });

            modelBuilder.Entity<HistorialClaveUsuariosV2>(entity =>
            {
                entity.ToTable("UserPasswordHistory");
                entity.HasKey(u => u.PasswordHistoryId);
                entity.HasIndex(h => h.AuditLogId).IsUnique();
                entity.Property(o => o.Password).IsRequired().HasColumnType("varchar").HasMaxLength(100);
            });

            modelBuilder.Entity<HistorialUsuarioUsuariosV2>(history =>
            {
                history.ToTable("UserUsernameHistory");
                history.HasKey(h => h.UsernameHistoryId);
                history.HasIndex(h => h.AuditLogId).IsUnique();
                history.Property(o => o.Username).IsRequired().HasColumnType("varchar").HasMaxLength(100);
            });

            modelBuilder.Entity<ModelosV2>(models =>
            {
                models.ToTable("Models");
                models.HasKey(u => u.ModelId);
                models.HasMany(a => a.ValidationRules).WithOne(a => a.Models).HasForeignKey(a => a.ModelId);
                models.Property(o => o.ModelName).IsRequired().HasColumnType("varchar").HasMaxLength(200);
                models.Property(o => o.Description).HasColumnType("varchar").HasMaxLength(300);
            });

            modelBuilder.Entity<InputsV2>(inputs =>
            {
                inputs.ToTable("Inputs");
                inputs.HasKey(u => u.InputId);
                inputs.HasMany(a => a.ValidationRules).WithOne(a => a.Inputs).HasForeignKey(a => a.InputId);
                inputs.Property(o => o.InputName).IsRequired().HasColumnType("varchar").HasMaxLength(200);
                inputs.Property(o => o.Description).HasColumnType("varchar").HasMaxLength(300);
            });

            modelBuilder.Entity<ReglaValidacionV2>(regla =>
            {
                regla.ToTable("ValidationRules");
                regla.HasKey(v => v.ValidationRuleId);
                regla.Property(v => v.CreatedDate).IsRequired().HasDefaultValueSql("GetDate()");
                regla.Property(o => o.ValidationRuleName).IsRequired().HasColumnType("varchar").HasMaxLength(200);
                regla.Property(o => o.ValidationRuleText).IsRequired().HasColumnType("varchar").HasMaxLength(500);
                regla.Property(o => o.RegularExpression).HasColumnType("varchar").HasMaxLength(1000);
            });
        }
    }
}
