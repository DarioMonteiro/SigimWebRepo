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
    public class OrdemCompraConfiguration : EntityTypeConfiguration<Domain.Entity.OrdemCompra.OrdemCompra>
    {
        public OrdemCompraConfiguration()
        {
            ToTable("OrdemCompra", "OrdemCompra");

            Property(l => l.Id)
                .HasColumnName("codigo");

            Property(l => l.Data)
                .HasColumnName("dataOrdemCompra");

            Property(l => l.Situacao)
                .HasColumnName("situacao");

            HasMany(l => l.ListaItens)
                .WithRequired(l => l.OrdemCompra);
        }
    }
}