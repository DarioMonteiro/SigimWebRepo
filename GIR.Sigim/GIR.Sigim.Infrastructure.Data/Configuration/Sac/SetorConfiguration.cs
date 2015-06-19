using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sac;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Sac
{
    public class SetorConfiguration : EntityTypeConfiguration<Setor>
    {
        public SetorConfiguration()
        {
            ToTable("Setor", "Sac");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Descricao)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("descricao")
                .HasColumnOrder(2);

          }
    }
}