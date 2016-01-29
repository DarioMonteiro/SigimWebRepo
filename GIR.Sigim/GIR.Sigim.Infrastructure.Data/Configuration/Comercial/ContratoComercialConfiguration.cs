using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Comercial;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Comercial
{
    public class ContratoComercialConfiguration : EntityTypeConfiguration<ContratoComercial>
    {
        public ContratoComercialConfiguration()
        {
            ToTable("Contrato","Comercial");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.UnidadeId)
                .IsRequired()
                .HasColumnName("unidade")
                .HasColumnOrder(2);

            HasRequired<Unidade>(l => l.Unidade)
                .WithMany(c => c.ListaContratoComercial)
                .HasForeignKey(l => l.UnidadeId);

            Property(l => l.TipoContrato)
                .IsRequired()
                .HasColumnName("tipoContrato")
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnOrder(3);

            Property(l => l.SituacaoContrato)
                .IsRequired()
                .HasColumnName("situacaoContrato")
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnOrder(4);

            Ignore(l => l.VendaId);

            HasRequired(l => l.Venda).WithRequiredPrincipal(l => l.Contrato);

            Ignore(l => l.VendaParticipanteId);
            HasRequired(l => l.VendaParticipante).WithRequiredPrincipal(l => l.Contrato);

        }

    }
}
