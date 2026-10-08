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

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Varre todas as entidades que estão sendo modificadas ou adicionadas
            foreach (var entry in ChangeTracker.Entries())
            {
                // Verifica se a entidade manipulada tem as propriedades de auditoria
                // (Se você criar outras classes depois, pode usar uma Interface aqui)
                if (entry.Entity is Veiculo veiculo)
                {
                    if (entry.State == EntityState.Added)
                    {
                        // Se for um INSERT, preenche a data de cadastro
                        veiculo.DataCadastro = DateTime.Now;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        // Se for UPDATE (Edit ou Soft Delete), preenche a data de atualização
                        veiculo.DataAtualizacao = DateTime.Now;

                        // Impede que o EF Core altere a DataCadastro original acidentalmente
                        entry.Property(nameof(veiculo.DataCadastro)).IsModified = false;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
