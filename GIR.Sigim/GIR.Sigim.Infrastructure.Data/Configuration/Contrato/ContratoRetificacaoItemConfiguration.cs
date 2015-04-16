using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Contrato
{
    public class ContratoRetificacaoItemConfiguration : EntityTypeConfiguration<ContratoRetificacaoItem>
    {
        public ContratoRetificacaoItemConfiguration()
        {
            ToTable("ContratoRetificacaoItem", "Contrato");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.ContratoId)
                .IsRequired() 
                .HasColumnName("contrato")
                .HasColumnOrder(2);

            //HasRequired(l => l.Contrato)
            //    .WithMany(c => c.ListaContratoRetificacaoItem).HasForeignKey(l => l.ContratoId);

            Property(l => l.ContratoRetificacaoId)
                .IsRequired()
                .HasColumnName("contratoRetificacao")
                .HasColumnOrder(3);

            HasRequired(l => l.ContratoRetificacao)
                .WithMany(c => c.ListaContratoRetificacaoItem)
                .HasForeignKey(l => l.ContratoRetificacaoId);

            Property(l => l.Sequencial)
                .IsRequired()
                .HasColumnName("sequencial")
                .HasColumnType("smallint")
                .HasColumnOrder(4);

            Property(l => l.ComplementoDescricao)
                .HasColumnName("complementoDescricao")
                .HasMaxLength(80)
                .HasColumnOrder(5);

            Property(l => l.NaturezaItem)
                .HasColumnName("naturezaItem")
                .IsRequired() 
                .HasColumnOrder(6);

            Property(l => l.ServicoId)
                .IsRequired()
                .HasColumnName("servico")
                .HasColumnOrder(7);

            HasRequired(l => l.Servico)
                .WithMany(c => c.ListaContratoRetificacaoItem)
                .HasForeignKey(l => l.ServicoId);

            Property(l => l.Quantidade)
                .HasColumnName("quantidade")
                .HasPrecision(18, 5)
                .IsRequired()
                .HasColumnOrder(8);

            Property(l => l.PrecoUnitario)
                .HasColumnName("precoUnitario")
                .HasPrecision(18, 5)
                .IsRequired()
                .HasColumnOrder(9);

            Property(l => l.ValorItem)
                .HasColumnName("valorItem")
                .HasPrecision(18, 5)
                .HasColumnOrder(10);

            Property(l => l.CodigoClasse)
                .HasColumnName("classe")
                .IsRequired()
                .HasColumnOrder(11);

            HasRequired(l => l.Classe)
                .WithMany(c => c.ListaContratoRetificacaoItem)
                .HasForeignKey(l => l.CodigoClasse);

            Property(l => l.RetencaoItem)
                .HasColumnName("retencaoItem")
                .HasPrecision(18, 5)
                .HasColumnOrder(12);

            Property(l => l.BaseRetencaoItem)
                .HasColumnName("baseRetencaoItem")
                .HasPrecision(18, 5)
                .HasColumnOrder(13);

            Property(l => l.RetencaoPrazoResgate)
                .HasColumnName("retencaoPrazoResgate")
                .HasColumnOrder(14);

            Property(l => l.Alterado)
                .HasColumnName("alterado")
                .HasColumnType("bit") 
                .HasColumnOrder(15);

            Property(l => l.RetencaoTipoCompromissoId)
                .HasColumnName("retencaoTipoCompromisso")
                .HasColumnOrder(16);

            HasOptional(l => l.RetencaoTipoCompromisso)
                .WithMany(c => c.ListaContratoRetificacaoItem)
                .HasForeignKey(l => l.RetencaoTipoCompromissoId); 

        }
    }
}
