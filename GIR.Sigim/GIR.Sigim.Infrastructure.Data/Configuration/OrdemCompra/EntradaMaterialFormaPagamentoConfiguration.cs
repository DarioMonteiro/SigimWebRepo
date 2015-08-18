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
    public class EntradaMaterialFormaPagamentoConfiguration : EntityTypeConfiguration<EntradaMaterialFormaPagamento>
    {
        public EntradaMaterialFormaPagamentoConfiguration()
        {
            ToTable("EntradaMaterialFormaPagamento", "OrdemCompra");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.EntradaMaterialId)
                .HasColumnName("entradaMaterial")
                .HasColumnOrder(2);

            HasOptional(l => l.EntradaMaterial)
                .WithMany(l => l.ListaFormaPagamento)
                .HasForeignKey(l => l.EntradaMaterialId);

            Property(l => l.OrdemCompraFormaPagamentoId)
                .HasColumnName("ordemCompraFormaPagamento")
                .HasColumnOrder(3);

            HasRequired(l => l.OrdemCompraFormaPagamento)
                .WithMany(l => l.ListaEntradaMaterialFormaPagamento)
                .HasForeignKey(l => l.OrdemCompraFormaPagamentoId);

            Property(l => l.Data)
                .HasColumnName("data")
                .HasColumnOrder(4);

            Property(l => l.Valor)
                .HasPrecision(18, 5)
                .HasColumnName("valor")
                .HasColumnOrder(5);

            Property(l => l.TipoCompromissoId)
                .HasColumnName("tipoCompromisso")
                .HasColumnOrder(6);

            HasOptional(l => l.TipoCompromisso)
                .WithMany(l => l.ListaEntradaMaterialFormaPagamento)
                .HasForeignKey(l => l.TipoCompromissoId);

            Property(l => l.TituloPagarId)
                .HasColumnName("tituloPagar")
                .HasColumnOrder(7);

            HasOptional(l => l.TituloPagar)
                .WithMany(l => l.ListaEntradaMaterialFormaPagamento)
                .HasForeignKey(l => l.TituloPagarId);
        }
    }
}