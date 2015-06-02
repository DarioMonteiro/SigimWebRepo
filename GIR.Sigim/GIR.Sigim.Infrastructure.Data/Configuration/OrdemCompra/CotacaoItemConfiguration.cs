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
    public class CotacaoItemConfiguration : EntityTypeConfiguration<CotacaoItem>
    {
        public CotacaoItemConfiguration()
        {
            ToTable("CotacaoItem", "OrdemCompra");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.CotacaoId)
                .HasColumnName("cotacao")
                .HasColumnOrder(2);

            HasRequired(l => l.Cotacao)
                .WithMany(l => l.ListaItens);

            Property(l => l.RequisicaoMaterialItemId)
                .HasColumnName("requisicaoMaterialItem")
                .HasColumnOrder(3);

            HasRequired(l => l.RequisicaoMaterialItem)
                .WithMany(l => l.ListaCotacaoItem);

            //TODO: Relacionar com Vencedor
            //.HasColumnOrder(4);

            Property(l => l.Sequencial)
                .HasColumnName("sequencial")
                .HasColumnOrder(5);

            Property(l => l.Quantidade)
                .HasPrecision(18, 5)
                .HasColumnName("quantidade")
                .HasColumnOrder(6);

            Property(l => l.OrdemCompraItemUltimoPreco)
                .HasColumnName("ordemCompraItemUltimoPreco")
                .HasColumnOrder(7);

            Property(l => l.ValorUltimoPreco)
                .HasPrecision(18, 5)
                .HasColumnName("valorUltimoPreco")
                .HasColumnOrder(8);

            Property(l => l.DataUltimoPreco)
                .HasColumnName("dataUltimoPreco")
                .HasColumnOrder(9);

            Property(l => l.ValorOrcado)
                .HasPrecision(18, 5)
                .HasColumnName("valorOrcado")
                .HasColumnOrder(10);

            Property(l => l.LoginUsuarioEleicao)
                .HasMaxLength(50)
                .HasColumnName("usuarioEleicao")
                .HasColumnOrder(11);

            Property(l => l.DataEleicao)
                .HasColumnName("dataEleicao")
                .HasColumnOrder(12);
        }
    }
}