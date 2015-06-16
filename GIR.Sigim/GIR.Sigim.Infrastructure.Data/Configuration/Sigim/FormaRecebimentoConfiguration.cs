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
    public class FormaRecebimentoConfiguration : EntityTypeConfiguration<FormaRecebimento>
    {
        public FormaRecebimentoConfiguration()
        {
            ToTable("FormaRecebimento", "Sigim");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Descricao)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("descricao")
                .HasColumnOrder(2);

            Property(l => l.TipoRecebimento)
                .HasColumnName("tipoRecebimento")
                .HasColumnType("char")
                .HasColumnOrder(3);

            Property(l => l.Automatico)
                .HasColumnName("automatico")
                .HasColumnType("bit")
                .HasColumnOrder(4);

            Property(l => l.NumeroDias)
                .HasColumnName("numeroDias")
                .HasColumnOrder(5);

        }
    }
}
