using Microsoft.EntityFrameworkCore;
using WebApiDets.Modelo;

namespace WebApiDets.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pacientesmisael> Pacientesmisael { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pacientesmisael>(entity =>
            {
                entity.ToTable("pacientesmisael"); // 👈 nombre exacto de la tabla
                entity.HasKey(p => p.IdPaciente);
            });
        }
    }
}
