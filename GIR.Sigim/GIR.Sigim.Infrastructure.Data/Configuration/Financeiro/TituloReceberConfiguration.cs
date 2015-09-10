using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    public class TituloReceberConfiguration : AbstractTituloConfiguration<TituloReceber>
    {
        public TituloReceberConfiguration()
        {
            ToTable("TituloReceber", "Financeiro");

            HasRequired(l => l.Cliente)
                .WithMany(c => c.ListaTituloReceber)
                .HasForeignKey(l => l.ClienteId);

            Property(l => l.Situacao)
                .HasColumnName("situacao")
                .HasColumnType("smallint");
              //.HasColumnOrder(5);

            HasOptional(l => l.TipoCompromisso)
                .WithMany(c => c.ListaTituloReceber)
                .HasForeignKey(l => l.TipoCompromissoId);

            HasOptional(l => l.TipoDocumento)
                .WithMany(c => c.ListaTituloReceber)
                .HasForeignKey(l => l.TipoDocumentoId);

            HasMany<ContratoRetificacaoItemMedicao>(l => l.ListaContratoRetificacaoItemMedicao)
                .WithOptional(l => l.TituloReceber)
                .HasForeignKey(c => c.TituloReceberId);

            HasMany<ContratoRetencaoLiberada>(l => l.ListaContratoRetencaoLiberada)
                .WithOptional(c => c.TituloReceber)
                .HasForeignKey(c => c.TituloReceberId);

        }
    }
}
