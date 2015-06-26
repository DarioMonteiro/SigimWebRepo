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
    public class NCMConfiguration : EntityTypeConfiguration<NCM>
    {
        public NCMConfiguration()
        {
            ToTable("NCM", "Sigim");

            Ignore(l => l.Id);

            HasKey(l => l.Codigo);

            Property(l => l.Codigo)
                .HasMaxLength(10)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Descricao)
                .HasMaxLength(70)
                .HasColumnName("descricao")
                .HasColumnOrder(2);

            HasMany<Material>(l => l.ListaMaterial)
                .WithOptional(c => c.NCM)
                .HasForeignKey(c => c.CodigoNCM);


        }
    }
}