using DAL.DAO;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ModelContext : DbContext
    {
        public ModelContext(DbContextOptions<ModelContext> options) : base(options){ }

        public DbSet<UsuarioDAO> usuarioDAO { get; set; }
        public DbSet<CreditoDAO> creditoDAO { get; set; }
        public DbSet<TransaccionDAO> transaccionDAO { get; set; }
        public DbSet<CuentaDAO> cuentaDAO { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}