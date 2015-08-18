using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;   
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    public class HistoricoContabilConfiguration : EntityTypeConfiguration<HistoricoContabil>
    {
        public HistoricoContabilConfiguration()
        {
            ToTable("HistoricoContabil", "Financeiro");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Descricao)
                .HasColumnName("descricao")
                .IsRequired()
                .HasMaxLength(50) 
                .HasColumnOrder(2);

            Property(l => l.Tipo )
               .HasColumnName("tipo")
               .IsRequired()
               .HasColumnOrder(3);

            HasMany<TipoMovimento>(l => l.ListaTipoMovimento)
                .WithOptional(c => c.HistoricoContabil)
                .HasForeignKey(c => c.HistoricoContabilId);
        }
    }
}
