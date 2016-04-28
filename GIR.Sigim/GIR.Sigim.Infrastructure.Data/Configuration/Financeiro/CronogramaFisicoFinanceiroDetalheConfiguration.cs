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
    public class CronogramaFisicoFinanceiroDetalheConfiguration : EntityTypeConfiguration<CronogramaFisicoFinanceiroDetalhe>
    {
        public CronogramaFisicoFinanceiroDetalheConfiguration()
        {
            ToTable("CronogramaFisicoFinanceiroDetalhe", "Financeiro");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.CronogramaFisicoFinanceiroId)
                .IsRequired()
                .HasColumnName("CronogramaFisicoFinanceiro")
                .HasColumnOrder(2);

            HasRequired<CronogramaFisicoFinanceiro>(l => l.CronogramaFisicoFinanceiro)
                .WithMany(l => l.ListaCronogramaFisicoFinanceiroDetalhe)
                .HasForeignKey(l => l.CronogramaFisicoFinanceiroId);

            Property(l => l.CodigoClasse)
                .IsRequired()
                .HasMaxLength(18)
                .HasColumnName("classe")
                .HasColumnOrder(3);

            HasRequired<Classe>(l => l.Classe)
                .WithMany(l => l.ListaCronogramaFisicoFinanceiroDetalhe)
                .HasForeignKey(l => l.CodigoClasse);

            Property(l => l.Percentual)
                .IsRequired()
                .HasPrecision(18,5)
                .HasColumnName("percentual")
                .HasColumnOrder(4);
        }
    }
}
