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
    public class ContratoRetificacaoItemMedicaoConfiguration : EntityTypeConfiguration<ContratoRetificacaoItemMedicao>
    {
        public ContratoRetificacaoItemMedicaoConfiguration()
        {
            ToTable("ContratoRetificacaoItemMedicao", "Contrato");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.ContratoId)
                .IsRequired()
                .HasColumnName("contrato")
                .HasColumnOrder(2);

            //HasRequired<Domain.Entity.Contrato.Contrato>(l => l.Contrato)
            //    .WithMany(c => c.ListaContratoRetificacaoItemMedicao)
            //    .HasForeignKey(l => l.ContratoId);

            Property(l => l.ContratoRetificacaoId)
                .IsRequired()
                .HasColumnName("contratoRetificacao")
                .HasColumnOrder(3);

            HasRequired<Domain.Entity.Contrato.ContratoRetificacao>(l => l.ContratoRetificacao)
                .WithMany(c => c.ListaContratoRetificacaoItemMedicao)
                .HasForeignKey(l => l.ContratoRetificacaoId);

            Property(l => l.ContratoRetificacaoItemId)
                .IsRequired()
                .HasColumnName("contratoRetificacaoItem")
                .HasColumnOrder(4);

            HasRequired<Domain.Entity.Contrato.ContratoRetificacaoItem>(l => l.ContratoRetificacaoItem)
                .WithMany(c => c.ListaContratoRetificacaoItemMedicao)
                .HasForeignKey(l => l.ContratoRetificacaoItemId);

            Property(l => l.SequencialItem)
                .IsRequired()
                .HasColumnName("sequencialItem")
                .HasColumnOrder(5);

            Property(l => l.ContratoRetificacaoItemCronogramaId)
                .IsRequired()
                .HasColumnName("contratoRetificacaoItemCronograma")
                .HasColumnOrder(6);

            HasRequired<Domain.Entity.Contrato.ContratoRetificacaoItemCronograma>(l => l.ContratoRetificacaoItemCronograma)
                .WithMany(c => c.ListaContratoRetificacaoItemMedicao)
                .HasForeignKey(l => l.ContratoRetificacaoItemCronogramaId);

            Property(l => l.SequencialCronograma)
                .IsRequired()
                .HasColumnName("sequencialCronograma")
                .HasColumnOrder(7);

            Property(l => l.Situacao)
                .IsRequired()
                .HasColumnName("situacao")
                .HasColumnOrder(8);

            Property(l => l.Quantidade)
                .IsRequired()
                .HasColumnName("quantidade")
                .HasPrecision(18, 7)
                .HasColumnOrder(13);

            Property(l => l.Valor)
                .IsRequired()
                .HasColumnName("valor")
                .HasPrecision(18,5)
                .HasColumnOrder(14);

        }
    }
}
