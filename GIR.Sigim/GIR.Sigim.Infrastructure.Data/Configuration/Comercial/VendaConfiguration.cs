using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration; 
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Comercial;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Comercial
{
    public class VendaConfiguration : EntityTypeConfiguration<Venda>
    {
        public VendaConfiguration()
        {
            ToTable("Venda","Comercial");

            Property(l => l.Id)
                .HasColumnName("contrato")
                .HasColumnOrder(1);

            Property(l => l.DataVenda)
                .HasColumnName("dataVenda")
                .HasColumnOrder(2);
        }

    }
}
