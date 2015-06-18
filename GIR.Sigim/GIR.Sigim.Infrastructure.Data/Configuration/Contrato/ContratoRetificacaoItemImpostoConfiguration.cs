using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Contrato
{
    public class ContratoRetificacaoItemImpostoConfiguration : EntityTypeConfiguration<ContratoRetificacaoItemImposto>
    {
        public ContratoRetificacaoItemImpostoConfiguration()
        {
            ToTable("ContratoRetificacaoItemImposto", "Contrato");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.ContratoId)
                .IsRequired()
                .HasColumnName("contrato")
                .HasColumnOrder(2);

            HasRequired<Domain.Entity.Contrato.Contrato>(l => l.Contrato)
                .WithMany(c => c.ListaContratoRetificacaoItemImposto)
                .HasForeignKey(l => l.ContratoId);

            Property(l => l.ContratoRetificacaoId)
                .IsRequired()
                .HasColumnName("contratoRetificacao")
                .HasColumnOrder(3);

            HasRequired<Domain.Entity.Contrato.ContratoRetificacao>(l => l.ContratoRetificacao)
                .WithMany(c => c.ListaContratoRetificacaoItemImposto)
                .HasForeignKey(l => l.ContratoRetificacaoId);

            Property(l => l.ContratoRetificacaoItemId)
                .IsRequired()
                .HasColumnName("contratoRetificacaoItem")
                .HasColumnOrder(4);

            HasRequired<Domain.Entity.Contrato.ContratoRetificacaoItem>(l => l.ContratoRetificacaoItem)
                .WithMany(c => c.ListaContratoRetificacaoItemImposto)
                .HasForeignKey(l => l.ContratoRetificacaoItemId);

            Property(l => l.ImpostoFinanceiroId)
                .IsRequired()
                .HasColumnName("impostoFinanceiro")
                .HasColumnOrder(5);

            HasRequired<Domain.Entity.Financeiro.ImpostoFinanceiro>(l => l.ImpostoFinanceiro)
                .WithMany(c => c.ListaContratoRetificacaoItemImposto)
                .HasForeignKey(l => l.ImpostoFinanceiroId);

            Property(l => l.PercentualBaseCalculo)
                .IsRequired()
                .HasColumnName("percentualBaseCalculo")
                .HasPrecision(18, 5)
                .HasColumnOrder(6);
        }
    }
}
