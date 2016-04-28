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
    public class CronogramaFisicoFinanceiroConfiguration : EntityTypeConfiguration<CronogramaFisicoFinanceiro>
    {

        public CronogramaFisicoFinanceiroConfiguration()
        {
            ToTable("CronogramaFisicoFinanceiro", "Financeiro");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.DataElaboracao)
                .IsRequired()
                .HasColumnName("dataElaboracao")
                .HasColumnOrder(2);

            Property(l => l.CodigoCentroCusto)
                .IsRequired()
                .HasMaxLength(18)
                .HasColumnName("centroCusto")
                .HasColumnOrder(3);

            HasRequired<CentroCusto>(l => l.CentroCusto)
                .WithMany(l => l.ListaCronogramaFisicoFinanceiro)
                .HasForeignKey(l => l.CodigoCentroCusto);

            Property(l => l.Sequencial)
                .IsRequired()
                .HasColumnName("sequencial")
                .HasColumnOrder(4);

        }

    }
}
