using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Sigim
{
    public class AvaliacaoModeloConfiguration : EntityTypeConfiguration<AvaliacaoModelo>
    {
        public AvaliacaoModeloConfiguration()
        {
            ToTable("AvaliacaoModelo", "Sigim");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Descricao)
                .HasColumnName("descricao")
                .HasMaxLength(50)
                .HasColumnOrder(2);

            Property(l => l.MediaMinima)
                .HasColumnName("mediaMinima")
                .HasColumnOrder(3);

            Property(l => l.Validade)
                .HasColumnName("validade")
                .HasColumnOrder(4);

            Property(l => l.TipoValidade)
                .HasColumnName("tipoValidade")
                .HasColumnOrder(5);
        }
    }
}