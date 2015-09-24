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
    public class ImpostoPagarConfiguration : EntityTypeConfiguration<ImpostoPagar>
    {
        public ImpostoPagarConfiguration()
        {
            ToTable("ImpostoAPagar", "Financeiro");

            Property(l => l.Id)
                .HasColumnName("codigo");

            Property(l => l.TituloPagarId)
                .HasColumnName("tituloPagar");

            HasRequired(l => l.TituloPagar)
                .WithMany(l => l.ListaImpostoPagar)
                .HasForeignKey(l => l.TituloPagarId);

            Property(l => l.ImpostoFinanceiroId)
                .HasColumnName("impostoFinanceiro");

            HasRequired(l => l.ImpostoFinanceiro)
                .WithMany(l => l.ListaImpostoPagar)
                .HasForeignKey(l => l.ImpostoFinanceiroId);

            Property(l => l.BaseCalculo)
                .HasPrecision(18, 5)
                .HasColumnName("baseCalculo");

            Property(l => l.ValorImposto)
                .HasPrecision(18, 5)
                .HasColumnName("valorImposto");

            Property(l => l.TituloPagarImpostoId)
                .HasColumnName("tituloPagarImposto");

            //HasOptional(l => l.TituloPagarImposto)
            //    .WithMany(l => l.ListaImpostoPagarTituloImposto)
            //    .HasForeignKey(l => l.TituloPagarImpostoId);

            Property(l => l.DataEmissaoPagamentoTituloOrigem)
                .HasColumnName("dataEmissaoOrigem");
        }
    }
}