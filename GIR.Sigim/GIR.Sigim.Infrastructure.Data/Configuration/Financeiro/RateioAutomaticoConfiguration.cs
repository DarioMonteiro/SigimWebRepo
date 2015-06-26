using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    public class RateioAutomaticoConfiguration : EntityTypeConfiguration<RateioAutomatico>
    {
        public RateioAutomaticoConfiguration()
        {
            ToTable("RateioAutomatico", "Financeiro");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.TipoRateioId)
               .HasColumnName("tipoRateio")
               .HasColumnOrder(2);

            HasRequired<TipoRateio>(l => l.TipoRateio)
                .WithMany(l => l.ListaRateioAutomatico)
                .HasForeignKey(l => l.TipoRateioId);

             Property(l => l.ClasseId )
                .HasColumnName("classe")
                .HasColumnOrder(3);

            HasOptional<Classe>(l => l.Classe)
                .WithMany(l => l.ListaRateioAutomatico)
                .HasForeignKey(l => l.ClasseId);
            
             Property(l => l.CentroCustoId)
                .HasColumnName("centroCusto")
                .HasColumnOrder(4);

             HasOptional<CentroCusto>(l => l.CentroCusto)
                 .WithMany(l => l.ListaRateioAutomatico)
                 .HasForeignKey(l => l.CentroCustoId);

             Property(l => l.Percentual)
                .HasColumnName("percentual")
                .HasColumnOrder(5);
        }
    }
}
