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
    public class ContratoRetificacaoItemCronogramaConfiguration : EntityTypeConfiguration<ContratoRetificacaoItemCronograma>
    {
        public ContratoRetificacaoItemCronogramaConfiguration()
        {
            ToTable("ContratoRetificacaoItemCronograma", "Contrato");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.ContratoId)
                .IsRequired()
                .HasColumnName("contrato")
                .HasColumnOrder(2);

            HasRequired<Domain.Entity.Contrato.Contrato>(l => l.Contrato)
                .WithMany(c => c.ListaContratoRetificacaoItemCronograma)
                .HasForeignKey(l => l.ContratoId);


            Property(l => l.ContratoRetificacaoId)
                .IsRequired()
                .HasColumnName("contratoRetificacao")
                .HasColumnOrder(3);

            HasRequired<Domain.Entity.Contrato.ContratoRetificacao>(l => l.ContratoRetificacao)
                .WithMany(c => c.ListaContratoRetificacaoItemCronograma)
                .HasForeignKey(l => l.ContratoRetificacaoId);

            Property(l => l.ContratoRetificacaoItemId)
                .IsRequired()
                .HasColumnName("contratoRetificacaoItem")
                .HasColumnOrder(4);

            HasRequired<Domain.Entity.Contrato.ContratoRetificacaoItem>(l => l.ContratoRetificacaoItem)
                .WithMany(c => c.ListaContratoRetificacaoItemCronograma)
                .HasForeignKey(l => l.ContratoRetificacaoItemId);

            Property(l => l.Sequencial)
                .IsRequired() 
                .HasColumnName("sequencial")
                .HasColumnOrder(5);

            Property(l => l.Descricao)
                .IsRequired()
                .HasColumnName("descricao")
                .HasMaxLength(80)
                .HasColumnOrder(6);

            Property(l => l.DataInicial)
                .IsRequired()
                .HasColumnName("dataInicial")
                .HasColumnOrder(7);

            Property(l => l.DataFinal)
                .IsRequired()
                .HasColumnName("dataFinal")
                .HasColumnOrder(8);

            Property(l => l.DataVencimento)
                .IsRequired()
                .HasColumnName("dataVencimento")
                .HasColumnOrder(9);

            Property(l => l.Quantidade)
                .IsRequired()
                .HasColumnName("quantidade")
                .HasPrecision(18,7)
                .HasColumnOrder(10);

            Property(l => l.PercentualExecucao)
                .IsRequired()
                .HasColumnName("percentualExecucao")
                .HasPrecision(18, 5)
                .HasColumnOrder(11);

            Property(l => l.Valor)
                .IsRequired()
                .HasColumnName("valor")
                .HasPrecision(18, 5)
                .HasColumnOrder(12);

        HasMany<ContratoRetificacaoProvisao>(l => l.ListaContratoRetificacaoProvisao)
            .WithOptional(c => c.ContratoRetificacaoItemCronograma)
            .HasForeignKey(c => c.ContratoRetificacaoItemCronogramaId);

        HasMany<ContratoRetificacaoItemMedicao>(l => l.ListaContratoRetificacaoItemMedicao)
            .WithRequired(c => c.ContratoRetificacaoItemCronograma)
            .HasForeignKey(c => c.ContratoRetificacaoItemCronogramaId);

        }
    }
}
