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
    public class TipoDocumentoConfiguration : EntityTypeConfiguration<TipoDocumento>
    {
        public TipoDocumentoConfiguration()
        {
            ToTable("TipoDocumento", "Financeiro");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Sigla)
                .HasColumnName("sigla")
                .IsRequired()
                .HasMaxLength(5)
                .HasColumnOrder(2);

            Property(l => l.Descricao)
                .HasColumnName("descricao")
                .IsRequired()
                .HasMaxLength(50) 
                .HasColumnOrder(3);

            HasMany<TituloPagar>(l => l.ListaTituloPagar)
                .WithOptional(l => l.TipoDocumento)
                .HasForeignKey(c => c.TipoDocumentoId);

            HasMany<TituloReceber>(l => l.ListaTituloReceber)
                .WithOptional(l => l.TipoDocumento)
                .HasForeignKey(c => c.TipoDocumentoId);

            HasMany<ContratoRetificacaoItemMedicao>(l => l.ListaContratoRetificacaoItemMedicao)
                .WithRequired(l => l.TipoDocumento)
                .HasForeignKey(c => c.TipoDocumentoId);

            HasMany<ContratoRetencaoLiberada>(l => l.ListaContratoRetencaoLiberada)
                .WithRequired(c => c.TipoDocumento)
                .HasForeignKey(c => c.TipoDocumentoId);
        }
    }
}
