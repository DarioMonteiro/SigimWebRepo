using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Admin;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    public class ParametrosUsuarioFinanceiroConfiguration : EntityTypeConfiguration<ParametrosUsuarioFinanceiro>
    {
        public ParametrosUsuarioFinanceiroConfiguration()
        {
            ToTable("ParametrosUsuario","Financeiro");

            Property(l => l.Id)
                .HasColumnName("usuario");

            Property(l => l.TipoImpressora)
                .HasMaxLength(50)
                .HasColumnName("tipoImpressora");

            Property(l => l.PortaSerial)
                .HasMaxLength(50)
                .HasColumnName("portaSerial");

            Ignore(l => l.TipoImpressoraEscolhida);
        }
    }
}
