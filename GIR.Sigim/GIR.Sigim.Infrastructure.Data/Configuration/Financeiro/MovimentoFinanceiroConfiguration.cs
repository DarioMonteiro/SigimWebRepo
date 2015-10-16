using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    public class MovimentoFinanceiroConfiguration : EntityTypeConfiguration<MovimentoFinanceiro>
    {
        public MovimentoFinanceiroConfiguration()
        {
            ToTable("Movimento", "Financeiro");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.TipoMovimentoId)
                .IsRequired()
                .HasColumnName("tipoMovimento")
                .HasColumnOrder(2);

            HasRequired<TipoMovimento>(l => l.TipoMovimento)
                .WithMany(c => c.ListaMovimentoFinanceiro)
                .HasForeignKey(l => l.TipoMovimentoId);

        }
    }
}
