using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.CredCob;

namespace GIR.Sigim.Infrastructure.Data.Configuration.CredCob
{
    public class VerbaCobrancaConfiguration : EntityTypeConfiguration<VerbaCobranca>
    {
        public VerbaCobrancaConfiguration()
        {
            ToTable("VerbaCobranca", "CredCob");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Descricao)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("descricao")
                .HasColumnOrder(2);

            Property(l => l.CodigoClasse)
                .HasMaxLength(18)
                .HasColumnName("classe")
                .HasColumnOrder(3);

            HasOptional<Domain.Entity.Financeiro.Classe>(l => l.Classe)
                .WithMany(c => c.ListaVerbaCobranca)
                .HasForeignKey(l => l.CodigoClasse);

            Property(l => l.Automatico)
                .HasColumnType("bit")
                .HasColumnName("automatico")
                .HasColumnOrder(4);
        }

    }
}
