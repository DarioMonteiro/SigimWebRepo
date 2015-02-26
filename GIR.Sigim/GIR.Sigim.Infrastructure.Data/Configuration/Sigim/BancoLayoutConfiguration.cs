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
    public class BancoLayoutConfiguration : EntityTypeConfiguration<BancoLayout>
    {
        public BancoLayoutConfiguration()
        {
            ToTable("BancoLayout", "Sigim");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Descricao)
                .HasMaxLength(50)
                .HasColumnName("descricao")
                .HasColumnOrder(2);

            Property(l => l.BancoId)
                .HasColumnName("codigoBC")
                .HasColumnOrder(3);

            Property(l => l.Padrao)
                .IsRequired()
                .HasColumnName("padrao")
                .HasColumnOrder(4);

            Property(l => l.Tipo)
                .HasColumnName("tipo")
                .HasColumnOrder(5);

            Property(l => l.DesconsideraPosicao)
                .HasColumnName("desconsideraPosicao");

            HasOptional(l => l.Banco)
                .WithMany(l => l.ListaBancoLayout);
        }
    }
}