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
    public class IndiceFinanceiroConfiguration : EntityTypeConfiguration<IndiceFinanceiro>
    {
        public IndiceFinanceiroConfiguration()
        {
            ToTable("IndiceFinanceiro", "Sigim");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Indice)
                .HasMaxLength(50)
                .HasColumnName("indice")
                .HasColumnOrder(2);

            Property(l => l.Classe)
                .HasMaxLength(50)
                .HasColumnName("classe")
                .HasColumnOrder(3);

            Property(l => l.Data)
                .HasColumnName("data")
                .HasColumnOrder(4);

            Property(l => l.Periodicidade)
                .HasMaxLength(50)
                .HasColumnName("periodicidade")
                .HasColumnOrder(5);

            Property(l => l.Descricao)
                .HasMaxLength(50)
                .HasColumnName("descricao")
                .HasColumnOrder(6);

            Property(l => l.FonteOrigem)
                .HasMaxLength(50)
                .HasColumnName("fonteOrigem")
                .HasColumnOrder(7);

            Property(l => l.Status)
                .HasMaxLength(50)
                .HasColumnName("status")
                .HasColumnOrder(8);

            Property(l => l.Dissidio)
                .HasColumnType("bit")
                .HasColumnName("dissidio")
                .HasColumnOrder(9);
        }
    }
}
