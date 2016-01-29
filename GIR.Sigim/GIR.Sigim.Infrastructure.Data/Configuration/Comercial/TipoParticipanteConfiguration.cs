using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Comercial;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Comercial
{
    public class TipoParticipanteConfiguration : EntityTypeConfiguration<TipoParticipante>
    {
        public TipoParticipanteConfiguration()
        {
            ToTable("TipoParticipante", "Comercial");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Descricao)
                .IsRequired()
                .HasColumnName("descricao")
                .HasMaxLength(50)
                .HasColumnOrder(2);

            Property(l => l.Automatico )
                .HasColumnName("automatico")
                .HasColumnType("bit")
                .HasColumnOrder(3);
        }
    }
}
