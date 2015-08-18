using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Estoque;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    public class EstoqueMaterialConfiguration : EntityTypeConfiguration<EstoqueMaterial>
    {
        public EstoqueMaterialConfiguration()
        {
            ToTable("EstoqueMaterial", "Estoque");

            Property(l => l.Id)
                .HasColumnName("codigo");

            Property(l => l.EstoqueId)
                .HasColumnName("estoque");

            HasRequired(l => l.Estoque)
                .WithMany(l => l.ListaEstoqueMaterial)
                .HasForeignKey(l => l.EstoqueId);

            Property(l => l.MaterialId)
                .HasColumnName("material");

            HasOptional(l => l.Material)
                .WithMany(l => l.ListaEstoqueMaterial)
                .HasForeignKey(l => l.MaterialId);

            Property(l => l.Quantidade)
                .HasPrecision(18, 5)
                .HasColumnName("quantidade");

            Property(l => l.Valor)
                .HasPrecision(18, 5)
                .HasColumnName("valor");

            Property(l => l.QuantidadeTemporaria)
                .HasPrecision(18, 5)
                .HasColumnName("quantidadeTemporaria");
        }
    }
}