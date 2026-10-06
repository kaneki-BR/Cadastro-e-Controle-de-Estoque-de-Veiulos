using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Revemar.Domain.Entities;

namespace Revemar.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Veiculo> Veiculos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapeamento e precisão decimal no Oracle
            modelBuilder.Entity<Veiculo>(entity =>
            {
                entity.ToTable("VEICULOS");
                entity.HasKey(v => v.Id);

                entity.Property(v => v.Preco)
                      .HasColumnType("DECIMAL(18,2)");

                entity.Property(v => v.Marca)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(v => v.Modelo)
                      .IsRequired()
                      .HasMaxLength(50);
            });
        }
    }
}
