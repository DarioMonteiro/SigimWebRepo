using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Sigim
{
    public class CifFobConfiguration : EntityTypeConfiguration<CifFob>
    {
        public CifFobConfiguration() 
        {
            ToTable("CIFFOB", "Sigim");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Descricao)
                .HasColumnName("descricao")
                .HasMaxLength(50)
                .HasColumnOrder(2);

            Property(l => l.CodigoInterno)
                .HasColumnName("codigoInterno")
                .HasColumnOrder(3);
        }
    }
}
