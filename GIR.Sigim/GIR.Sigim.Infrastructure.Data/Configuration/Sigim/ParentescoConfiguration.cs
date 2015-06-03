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
    public class ParentescoConfiguration : EntityTypeConfiguration<Parentesco>
    {
        public ParentescoConfiguration()
        {
            ToTable("Parentesco", "Sigim");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Descricao)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("descricao")
                .HasColumnOrder(2);

            Property(l => l.Automatico)
                .HasColumnName("automatico")
                .HasColumnOrder(3);

        }
    }
}