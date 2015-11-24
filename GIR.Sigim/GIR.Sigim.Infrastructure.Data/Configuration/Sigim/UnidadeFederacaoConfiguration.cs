using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;


namespace GIR.Sigim.Infrastructure.Data.Configuration.Sigim
{
    public class UnidadeFederacaoConfiguration : EntityTypeConfiguration<UnidadeFederacao>
    {
        public UnidadeFederacaoConfiguration()
        {
            ToTable("UnidadeFederacao", "Sigim");

            Ignore(l => l.Id);

            HasKey(l => l.Sigla);

            Property(l => l.Sigla)
                .IsRequired()
                .HasMaxLength(2)
                .HasColumnName("sigla")
                .HasColumnOrder(1);

            Property(l => l.NomeUnidadeFederacao)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("unidadeFederacao")
                .HasColumnOrder(2);

            Property(l => l.CodigoIBGE)
                .HasColumnName("codigoIBGE")
                .HasColumnOrder(3);
        }
    }
}
