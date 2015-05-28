using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    public class TituloReceberConfiguration : AbstractTituloConfiguration<TituloReceber>
    {
        public TituloReceberConfiguration()
        {
            ToTable("TituloReceber", "Financeiro");

            HasRequired(l => l.Cliente)
                .WithMany(c => c.ListaTituloReceber);

            HasOptional(l => l.TipoCompromisso)
                .WithMany(c => c.ListaTituloReceber);

            HasOptional(l => l.TipoDocumento)
                .WithMany(c => c.ListaTituloReceber);

        }
    }
}
