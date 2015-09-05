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
    public class FeriadoConfiguration : EntityTypeConfiguration<Feriado>
    {

        public FeriadoConfiguration()
        {
            ToTable("Feriado", "Sigim");

            Property(l => l.Id)
                .HasColumnName("codigo");

            Property(l => l.Data)
                .HasColumnName("data");

            Property(l => l.Descricao)
                .HasColumnName("descricao")
                .HasMaxLength(50);
        }
    }
}