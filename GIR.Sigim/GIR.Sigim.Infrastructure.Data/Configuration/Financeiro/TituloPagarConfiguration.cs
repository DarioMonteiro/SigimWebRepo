using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    public class TituloPagarConfiguration : AbstractTituloConfiguration<TituloPagar> 
    {
        public TituloPagarConfiguration()
        {
            ToTable("TituloPagar", "Financeiro");

            HasRequired(l => l.Cliente)
                .WithMany(c => c.ListaTituloPagar)
                .HasForeignKey(l => l.ClienteId);

            HasOptional(l => l.TipoCompromisso)
                .WithMany(c => c.ListaTituloPagar)
                .HasForeignKey(l => l.TipoCompromissoId);

            HasOptional<TipoDocumento>(l => l.TipoDocumento)
                .WithMany(c => c.ListaTituloPagar)
                .HasForeignKey(l => l.TipoDocumentoId);

            HasMany<ContratoRetificacaoItemMedicao>(l => l.ListaContratoRetificacaoItemMedicao)
                .WithOptional(c => c.TituloPagar)
                .HasForeignKey(c => c.TituloPagarId);

            Property(l => l.TituloPaiId)
                .HasColumnName("tituloPai");

            HasOptional(l => l.TituloPai)
                .WithMany(l => l.ListaFilhos)
                .HasForeignKey(l => l.TituloPaiId);
        }
    }
}