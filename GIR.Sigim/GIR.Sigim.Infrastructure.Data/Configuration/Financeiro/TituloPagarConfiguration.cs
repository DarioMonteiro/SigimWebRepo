using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    public class TituloPagarConfiguration : AbstractTituloConfiguration<TituloPagar> 
    {
        public TituloPagarConfiguration()
        {
            ToTable("TituloPagar", "Financeiro");

            HasRequired(l => l.Cliente)
                .WithMany(c => c.ListaTituloPagar);

            HasOptional(l => l.TipoCompromisso)
                .WithMany(c => c.ListaTituloPagar);

            HasOptional(l => l.TipoDocumento)
                .WithMany(c => c.ListaTituloPagar);

        }
    }
}
