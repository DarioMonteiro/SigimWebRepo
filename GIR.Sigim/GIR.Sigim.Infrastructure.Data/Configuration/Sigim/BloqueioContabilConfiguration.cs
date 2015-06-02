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
    public class BloqueioContabilConfiguration : EntityTypeConfiguration<BloqueioContabil>
    {
        public BloqueioContabilConfiguration()
        {

            ToTable("BloqueioContabil", "Sigim");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.EmpreendimentoId)
                .HasColumnName("empreendimento")
                .HasColumnOrder(2);

            Property(l => l.BlocoId)
                .HasColumnName("bloco")
                .HasColumnOrder(3);

            Property(l => l.CodigoCentroCusto)
                .HasColumnName("centroCusto")
                .HasColumnOrder(4);

            HasOptional<GIR.Sigim.Domain.Entity.Financeiro.CentroCusto>(l => l.CentroCusto)
                .WithMany(c => c.ListaBloqueioContabil)
                .HasForeignKey(l => l.CodigoCentroCusto);

            Property(l => l.Data)
                .HasColumnName("data")
                .IsRequired()
                .HasColumnOrder(5);

            Property(l => l.Sistema)
                .HasColumnName("sistema")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnOrder(6);

            Property(l => l.UsuarioCadastro)
                .HasColumnName("usuarioCadastro")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnOrder(7);

            Property(l => l.DataCadastro)
                .HasColumnName("dataCadastro")
                .IsRequired()
                .HasColumnOrder(8);
        }
    }
}
