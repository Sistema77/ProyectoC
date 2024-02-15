using DAL.DAO;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ModelContext : DbContext
    {
        public ModelContext() { }

        public ModelContext(DbContextOptions<ModelContext> options) : base(options)
        { }

        public DbSet<UsuarioDAO> usuarioDAO { get; set; }
        public DbSet<CreditoDAO> creditoDAO { get; set; }
        public DbSet<TransaccionDAO> transaccionDAO { get; set; }
        public DbSet<CuentaDAO> cuentaDAO { get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=DefaultConnection");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<UsuarioDAO>(entity =>
            {
                entity.HasKey(e => e.id_usuario).HasName("usuarios_pkey");
                entity.ToTable("Usuario", "schemausuario");

                entity.Property(e => e.id_usuario).HasColumnName("id_usuario");
                entity.Property(e => e.email).HasColumnName("email");
                entity.Property(e => e.dni).HasColumnName("dni");
                entity.Property(e => e.expiracion_token).HasColumnName("expiracion_token").HasColumnType("timestamp(6) without time zone");
                entity.Property(e => e.fch_alta).HasColumnName("fch_alta").HasColumnType("timestamp(6) without time zone");
                entity.Property(e => e.foto).HasColumnName("foto");
                entity.Property(e => e.last_name).HasColumnName("last_name");
                entity.Property(e => e.name).HasColumnName("name");
                entity.Property(e => e.password).HasColumnName("password");
                entity.Property(e => e.tipo_usuario).HasColumnName("tipo_usuario");
                entity.Property(e => e.tlf).HasColumnName("tlf");
                entity.Property(e => e.token).HasColumnName("token");
                entity.Property(e => e.cuentaConfirmada).HasColumnName("cuentaConfirmada").HasDefaultValue(false);
            });*/



            base.OnModelCreating(modelBuilder);
        }

    }

}