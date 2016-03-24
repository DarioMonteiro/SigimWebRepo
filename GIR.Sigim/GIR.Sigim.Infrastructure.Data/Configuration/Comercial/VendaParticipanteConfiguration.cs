using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration; 
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Comercial;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Comercial
{
    public class VendaParticipanteConfiguration : EntityTypeConfiguration<VendaParticipante>
    {
        public VendaParticipanteConfiguration()
        {
            ToTable("VendaParticipante","Comercial");

            Ignore(l => l.Id);

            HasKey(l => new { l.ContratoId, l.ClienteId });

            Property(l => l.ContratoId)
                .HasColumnName("contrato");

            Property(l => l.ClienteId)
                .HasColumnName("cliente");

            HasRequired<ClienteFornecedor>(l => l.Cliente)
                .WithMany(c => c.ListaVendaParticipante)
                .HasForeignKey(l => l.ClienteId);

            Property(l => l.TipoParticipanteId)
                .HasColumnName("tipoParticipante");

            HasOptional<TipoParticipante>(l => l.TipoParticipante)
                .WithMany(c => c.ListaVendaParticipante)
                .HasForeignKey(l => l.TipoParticipanteId);

            HasRequired<ContratoComercial>(l => l.Contrato)
                .WithMany(c => c.ListaVendaParticipante)
                .HasForeignKey(l => l.ContratoId);

            Property(l => l.PercentualParticipacao)
                .HasPrecision(18, 2)
                .HasColumnName("percentualParticipacao");
        }
    }
}
