using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    public class ImpostoReceberConfiguration : EntityTypeConfiguration<ImpostoReceber>
    {
        public ImpostoReceberConfiguration()
        {
            ToTable("ImpostoReceber", "Financeiro");

            Property(l => l.Id)
                .HasColumnName("codigo");

            Property(l => l.TituloReceberId)
                .HasColumnName("tituloReceber");

            HasRequired(l => l.TituloReceber)
                .WithMany(l => l.ListaImpostoReceber)
                .HasForeignKey(l => l.TituloReceberId);

            Property(l => l.ImpostoFinanceiroId)
                .HasColumnName("impostoFinanceiro");

            HasRequired(l => l.ImpostoFinanceiro)
                .WithMany(l => l.ListaImpostoReceber)
                .HasForeignKey(l => l.ImpostoFinanceiroId);

            Property(l => l.BaseCalculo)
                .HasPrecision(18, 5)
                .HasColumnName("baseCalculo");

            Property(l => l.ValorImposto)
                .HasPrecision(18, 5)
                .HasColumnName("valorImposto");

            Property(l => l.DataEmissaoDocumento)
                .HasColumnName("dataEmissaoDocumento");

            Property(l => l.DataRecebimento)
                .HasColumnName("dataRecebimento");
        }

    }
}
