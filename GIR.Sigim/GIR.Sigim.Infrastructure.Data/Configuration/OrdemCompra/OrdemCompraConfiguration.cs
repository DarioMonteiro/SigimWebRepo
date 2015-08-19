using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Configuration.OrdemCompra
{
    public class OrdemCompraConfiguration : EntityTypeConfiguration<Domain.Entity.OrdemCompra.OrdemCompra>
    {
        public OrdemCompraConfiguration()
        {
            ToTable("OrdemCompra", "OrdemCompra");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.CodigoCentroCusto)
                .IsRequired()
                .HasMaxLength(18)
                .HasColumnName("centroCusto")
                .HasColumnOrder(2);

            HasRequired<CentroCusto>(l => l.CentroCusto)
                .WithMany(c => c.ListaOrdemCompra)
                .HasForeignKey(l => l.CodigoCentroCusto);

            Property(l => l.ClienteFornecedorId)
                .HasColumnName("clienteFornecedor")
                .HasColumnOrder(3);

            HasRequired<ClienteFornecedor>(l => l.ClienteFornecedor)
                .WithMany(c => c.ListaOrdemCompra)
                .HasForeignKey(l => l.ClienteFornecedorId);

            Property(l => l.Data)
                .HasColumnName("dataOrdemCompra")
                .HasColumnOrder(4);

            Property(l => l.Situacao)
                .HasColumnName("situacao")
                .HasColumnType("smallint")
                .HasColumnOrder(5);

            Property(l => l.PrazoEntrega)
                .HasColumnName("prazoEntrega")
                .HasColumnOrder(13);

            Property(l => l.EntradaMaterialFreteId)
                .HasColumnName("entradaMaterialFrete");

            HasOptional(l => l.EntradaMaterialFrete)
                .WithMany(c => c.ListaOrdemCompraFrete)
                .HasForeignKey(l => l.EntradaMaterialFreteId);

            HasMany(l => l.ListaItens)
                .WithRequired(l => l.OrdemCompra);
        }
    }
}