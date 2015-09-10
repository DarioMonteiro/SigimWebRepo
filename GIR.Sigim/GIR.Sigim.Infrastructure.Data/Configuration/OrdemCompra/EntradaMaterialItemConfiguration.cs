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
    public class EntradaMaterialItemConfiguration : EntityTypeConfiguration<EntradaMaterialItem>
    {
        public EntradaMaterialItemConfiguration()
        {
            ToTable("EntradaMaterialItem", "OrdemCompra");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.EntradaMaterialId)
                .HasColumnName("entradaMaterial")
                .HasColumnOrder(2);

            HasOptional(l => l.EntradaMaterial)
                .WithMany(l => l.ListaItens)
                .HasForeignKey(l => l.EntradaMaterialId);

            Property(l => l.OrdemCompraItemId)
                .HasColumnName("ordemCompraItem")
                .HasColumnOrder(3);

            HasRequired(l => l.OrdemCompraItem)
                .WithMany(l => l.ListaEntradaMaterialItem)
                .HasForeignKey(l => l.OrdemCompraItemId);

            Property(l => l.CodigoClasse)
                .HasMaxLength(18)
                .HasColumnName("classe")
                .HasColumnOrder(4);

            HasRequired(l => l.Classe)
                .WithMany(l => l.ListaEntradaMaterialItem)
                .HasForeignKey(l => l.CodigoClasse);

            Property(l => l.Sequencial)
                .HasColumnName("sequencial")
                .HasColumnOrder(5);

            Property(l => l.Quantidade)
                .HasPrecision(18, 5)
                .HasColumnName("quantidade")
                .HasColumnOrder(6);

            Property(l => l.ValorUnitario)
                .HasPrecision(18, 5)
                .HasColumnName("valorUnitario")
                .HasColumnOrder(7);

            Property(l => l.PercentualIPI)
                .HasPrecision(18, 5)
                .HasColumnName("percentualIPI")
                .HasColumnOrder(8);

            Property(l => l.PercentualDesconto)
                .HasPrecision(18, 5)
                .HasColumnName("percentualDesconto")
                .HasColumnOrder(9);

            Property(l => l.ValorTotal)
                .HasPrecision(18, 5)
                .HasColumnName("valorTotal")
                .HasColumnOrder(10);

            Property(l => l.BaseICMS)
                .HasPrecision(18, 5)
                .HasColumnName("baseICMS")
                .HasColumnOrder(11);

            Property(l => l.PercentualICMS)
                .HasPrecision(18, 5)
                .HasColumnName("percentualICMS")
                .HasColumnOrder(12);

            Property(l => l.BaseIPI)
                .HasPrecision(18, 5)
                .HasColumnName("baseIPI")
                .HasColumnOrder(13);

            Property(l => l.BaseICMSST)
                .HasPrecision(18, 5)
                .HasColumnName("baseICMSST")
                .HasColumnOrder(14);

            Property(l => l.PercentualICMSST)
                .HasPrecision(18, 5)
                .HasColumnName("percentualICMSST")
                .HasColumnOrder(15);

            Property(l => l.CodigoComplementoNaturezaOperacao)
                .HasMaxLength(10)
                .HasColumnName("complementoNaturezaOperacao")
                .HasColumnOrder(16);

            HasOptional(l => l.ComplementoNaturezaOperacao)
                .WithMany(l => l.ListaEntradaMaterialItem)
                .HasForeignKey(l => l.CodigoComplementoNaturezaOperacao);

            Property(l => l.CodigoComplementoCST)
                .HasMaxLength(10)
                .HasColumnName("complementoCST")
                .HasColumnOrder(17);

            HasOptional(l => l.ComplementoCST)
                .WithMany(l => l.ListaEntradaMaterialItem)
                .HasForeignKey(l => l.CodigoComplementoCST);

            Property(l => l.CodigoNaturezaReceita)
                .HasMaxLength(10)
                .HasColumnName("naturezaReceita")
                .HasColumnOrder(18);

            HasOptional(l => l.NaturezaReceita)
                .WithMany(l => l.ListaEntradaMaterialItem)
                .HasForeignKey(l => l.CodigoNaturezaReceita);
        }
    }
}