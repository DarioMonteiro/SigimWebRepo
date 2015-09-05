using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Infrastructure.Data.Configuration.OrdemCompra
{
    public class EntradaMaterialImpostoConfiguration : EntityTypeConfiguration<EntradaMaterialImposto>
    {
        public EntradaMaterialImpostoConfiguration()
        {
            ToTable("EntradaMaterialImposto", "OrdemCompra");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.EntradaMaterialId)
                .HasColumnName("entradaMaterial")
                .HasColumnOrder(2);

            HasOptional(l => l.EntradaMaterial)
                .WithMany(l => l.ListaImposto)
                .HasForeignKey(l => l.EntradaMaterialId);

            Property(l => l.ImpostoFinanceiroId)
                .HasColumnName("impostoFinanceiro")
                .HasColumnOrder(3);

            HasRequired(l => l.ImpostoFinanceiro)
                .WithMany(l => l.ListaEntradaMaterialImposto)
                .HasForeignKey(l => l.ImpostoFinanceiroId);

            Property(l => l.DataVencimento)
                .HasColumnName("dataVencimento")
                .HasColumnOrder(4);

            Property(l => l.BaseCalculo)
                .HasPrecision(18, 5)
                .HasColumnName("baseCalculo")
                .HasColumnOrder(5);

            Property(l => l.Valor)
                .HasPrecision(18, 5)
                .HasColumnName("valorImposto")
                .HasColumnOrder(6);

            Property(l => l.TituloPagarImpostoId)
                .HasColumnName("tituloPagarImposto")
                .HasColumnOrder(7);
        }
    }
}