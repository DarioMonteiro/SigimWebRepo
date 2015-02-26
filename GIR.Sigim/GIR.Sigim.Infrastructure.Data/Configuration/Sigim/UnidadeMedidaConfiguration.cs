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
    public class UnidadeMedidaConfiguration : EntityTypeConfiguration<UnidadeMedida>
    {
        public UnidadeMedidaConfiguration()
        {
            ToTable("UnidadeMedida", "Sigim");

            Ignore(l => l.Id);

            HasKey(l => l.Sigla);

            Property(l => l.Sigla)
                .HasMaxLength(6)
                .HasColumnName("siglaUnidade")
                .HasColumnOrder(1);

            Property(l => l.Descricao)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("descricao")
                .HasColumnOrder(2);

            HasMany(l => l.ListaMaterial)
                .WithOptional(l => l.UnidadeMedida);
        }
    }
}