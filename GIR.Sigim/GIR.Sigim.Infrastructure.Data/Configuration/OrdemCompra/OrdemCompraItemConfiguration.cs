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
    public class OrdemCompraItemConfiguration : EntityTypeConfiguration<OrdemCompraItem>
    {
        public OrdemCompraItemConfiguration()
        {
            ToTable("OrdemCompraItem", "OrdemCompra");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.OrdemCompraId)
                .HasColumnName("ordemCompra")
                .HasColumnOrder(2);

            HasRequired(l => l.OrdemCompra)
                .WithMany(l => l.ListaItens);

            Property(l => l.RequisicaoMaterialItemId)
                .HasColumnName("requisicaoMaterialItem")
                .HasColumnOrder(3);

            HasOptional(l => l.RequisicaoMaterialItem)
                .WithMany(l => l.ListaOrdemCompraItem);

            Property(l => l.CotacaoItemId)
                .HasColumnName("cotacaoItem")
                .HasColumnOrder(4);

            HasOptional(l => l.CotacaoItem)
                .WithMany(l => l.ListaOrdemCompraItem);

            Property(l => l.MaterialId)
                .HasColumnName("material")
                .HasColumnOrder(5);

            HasRequired(l => l.Material)
                .WithMany(l => l.ListaOrdemCompraItem);

            Property(l => l.CodigoClasse)
                .HasMaxLength(18)
                .HasColumnName("classe")
                .HasColumnOrder(6);

            HasRequired(l => l.Classe)
                .WithMany(l => l.ListaOrdemCompraItem)
                .HasForeignKey(l => l.CodigoClasse);

            Property(l => l.Sequencial)
                .HasColumnName("sequencial")
                .HasColumnOrder(7);

            Property(l => l.Complemento)
                .HasMaxLength(80)
                .HasColumnName("complementoDescricao")
                .HasColumnOrder(8);

            Property(l => l.Quantidade)
                .HasPrecision(18, 5)
                .HasColumnName("quantidade")
                .HasColumnOrder(9);

            Property(l => l.QuantidadeEntregue)
                .HasPrecision(18, 5)
                .HasColumnName("quantidadeEntregue")
                .HasColumnOrder(10);

            Property(l => l.ValorUnitario)
                .HasPrecision(18, 5)
                .HasColumnName("valorUnitario")
                .HasColumnOrder(11);

            Property(l => l.PercentualIPI)
                .HasPrecision(18, 5)
                .HasColumnName("percentualIPI")
                .HasColumnOrder(12);

            Property(l => l.PercentualDesconto)
                .HasPrecision(18, 5)
                .HasColumnName("percentualDesconto")
                .HasColumnOrder(13);

            Property(l => l.ValorTotalComImposto)
                .HasPrecision(18, 5)
                .HasColumnName("valorTotalComImposto")
                .HasColumnOrder(14);

            Property(l => l.ValorTotalItem)
                .HasPrecision(18, 5)
                .HasColumnName("valorTotalItem")
                .HasColumnOrder(15);
        }
    }
}