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
    public class OrdemCompraFormaPagamentoConfiguration : EntityTypeConfiguration<OrdemCompraFormaPagamento>
    {
        public OrdemCompraFormaPagamentoConfiguration()
        {
            ToTable("OrdemCompraFormaPagamento", "OrdemCompra");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.OrdemCompraId)
                .HasColumnName("ordemCompra")
                .HasColumnOrder(2);

            HasOptional(l => l.OrdemCompra)
                .WithMany(l => l.ListaOrdemCompraFormaPagamento)
                .HasForeignKey(l => l.OrdemCompraId);

            Property(l => l.Data)
                .HasColumnName("data")
                .HasColumnOrder(3);

            Property(l => l.Valor)
                .HasPrecision(18, 5)
                .HasColumnName("valor")
                .HasColumnOrder(4);

            Property(l => l.TipoCompromissoId)
                .HasColumnName("tipoCompromisso")
                .HasColumnOrder(5);

            HasOptional(l => l.TipoCompromisso)
                .WithMany(l => l.ListaOrdemCompraFormaPagamento)
                .HasForeignKey(l => l.TipoCompromissoId);

            Property(l => l.TituloPagarId)
                .HasColumnName("tituloPagar")
                .HasColumnOrder(6);

            HasOptional(l => l.TituloPagar)
                .WithMany(l => l.ListaOrdemCompraFormaPagamento)
                .HasForeignKey(l => l.TituloPagarId);

            Property(l => l.EhPagamentoAntecipado)
                .HasColumnName("pagamentoAntecipado")
                .HasColumnOrder(7);

            Property(l => l.EhUtilizada)
                .HasColumnName("utilizada")
                .HasColumnOrder(8);

            Property(l => l.EhAssociada)
                .HasColumnName("associada")
                .HasColumnOrder(9);
        }
    }
}