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
    public class ContratoRetencaoConfiguration : EntityTypeConfiguration<ContratoRetencao>
    {
        public ContratoRetencaoConfiguration()
        {
            ToTable("ContratoRetencao", "Contrato");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.ContratoId)
                .IsRequired()
                .HasColumnName("contrato")
                .HasColumnOrder(2);

            HasRequired<Domain.Entity.Contrato.Contrato>(l => l.Contrato)
                .WithMany(c => c.ListaContratoRetencao)
                .HasForeignKey(l => l.ContratoId);

            Property(l => l.ContratoRetificacaoId)
                .IsRequired()
                .HasColumnName("contratoRetificacao")
                .HasColumnOrder(3);

            HasRequired<ContratoRetificacao>(l => l.ContratoRetificacao)
                .WithMany(c => c.ListaContratoRetencao)
                .HasForeignKey(l => l.ContratoRetificacaoId);

            Property(l => l.ContratoRetificacaoItemId)
                .IsRequired()
                .HasColumnName("contratoRetificacaoItem")
                .HasColumnOrder(4);

            HasRequired<ContratoRetificacaoItem>(l => l.ContratoRetificacaoItem)
                .WithMany(c => c.ListaContratoRetencao)
                .HasForeignKey(l => l.ContratoRetificacaoItemId);

            Property(l => l.ContratoRetificacaoItemMedicaoId)
                .IsRequired()
                .HasColumnName("contratoRetificacaoItemMedicao")
                .HasColumnOrder(5);

            HasRequired<ContratoRetificacaoItemMedicao>(l => l.ContratoRetificacaoItemMedicao)
                .WithMany(c => c.ListaContratoRetencao)
                .HasForeignKey(l => l.ContratoRetificacaoItemMedicaoId);

            Property(l => l.ValorRetencao)
                .HasColumnName("valorRetencao")
                .HasPrecision(18, 5)
                .HasColumnOrder(6);

            HasMany<ContratoRetencaoLiberada>(l => l.ListaContratoRetencaoLiberada)
                .WithRequired(c => c.ContratoRetencao)
                .HasForeignKey(c => c.ContratoRetencaoId);

        }

    }
}
