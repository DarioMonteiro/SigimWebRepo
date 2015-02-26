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
    public class SituacaoMercadoriaConfiguration : EntityTypeConfiguration<SituacaoMercadoria>
    {
        public SituacaoMercadoriaConfiguration()
        {
            ToTable("SituacaoMercadoria", "Sigim");

            Ignore(l => l.Id);

            HasKey(l => l.Codigo);

            Property(l => l.Codigo)
                .HasMaxLength(10)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Descricao)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("descricao")
                .HasColumnOrder(2);

            HasMany(l => l.ListaMaterial)
                .WithOptional(l => l.SituacaoMercadoria);
        }
    }
}