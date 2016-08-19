using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.CredCob;

namespace GIR.Sigim.Infrastructure.Data.Configuration.CredCob
{
    public class MoedaConfiguration : EntityTypeConfiguration<Moeda>
    {
        public MoedaConfiguration()
        {
            ToTable("Moeda", "CredCob");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Simbolo)
                .HasMaxLength(50)
                .HasColumnName("simbolo")
                .HasColumnOrder(2);

            Property(l => l.Descricao)
                .HasMaxLength(50)
                .HasColumnName("descricao")
                .HasColumnOrder(3);
        }
    }
}
