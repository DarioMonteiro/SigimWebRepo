using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.CredCob;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Infrastructure.Data.Configuration.CredCob
{
    public class TituloMovimentoConfiguration : EntityTypeConfiguration<TituloMovimento>
    {
        public TituloMovimentoConfiguration()
        {
            ToTable("TituloMovimento", "CredCob");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.TituloCredCobId)
                .HasColumnName("titulo")
                .HasColumnOrder(2);

            HasOptional<TituloCredCob>(l => l.TituloCredCob)
                .WithMany(c => c.ListaTituloMovimento)
                .HasForeignKey(l => l.TituloCredCobId);

            Property(l => l.MovimentoFinanceiroId)
                .HasColumnName("movimento")
                .HasColumnOrder(3);

            HasOptional<MovimentoFinanceiro>(l => l.MovimentoFinanceiro)
                .WithMany(c => c.ListaTituloMovimento)
                .HasForeignKey(l => l.MovimentoFinanceiroId);

        }

    }
}
