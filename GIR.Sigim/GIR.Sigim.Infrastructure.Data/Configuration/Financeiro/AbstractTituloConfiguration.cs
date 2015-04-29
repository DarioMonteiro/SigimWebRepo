using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    public class AbstractTituloConfiguration<T> : EntityTypeConfiguration<T> where T : AbstractTitulo
    {
        public AbstractTituloConfiguration()
        {

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.ClienteID)
                .IsRequired()
                .HasColumnName("cliente")
                .HasColumnOrder(2);

            Property(l => l.TipoCompromissoId)
                .HasColumnName("tipoCompromisso")
                .HasColumnOrder(3);

            Property(l => l.Identificacao)
                .HasColumnName("identificacao")
                .HasMaxLength(70)
                .HasColumnOrder(4);

            Property(l => l.Situacao)
                .HasColumnName("situacao")
                .HasColumnType("smallint")
                .HasColumnOrder(5);

            Property(l => l.TipoDocumentoId)
                .HasColumnName("tipoDocumento")
                .HasColumnOrder(6);

            Property(l => l.Documento)
                .HasColumnName("documento")
                .HasMaxLength(10)
                .HasColumnOrder(7);

            Property(l => l.DataEmissaoDocumento)
                .HasColumnName("dataEmissaoDocumento")
                .HasColumnOrder(8);

            Property(l => l.DataVencimento)
                .HasColumnName("dataVencimento")
                .HasColumnOrder(9);

            Property(l => l.TipoTitulo)
                .HasColumnName("tipoTitulo")
                .IsRequired()
                .HasColumnType("smallint")
                .HasColumnOrder(10);

            Property(l => l.ValorTitulo)
                .HasColumnName("valorTitulo")
                .IsRequired()
                .HasPrecision(18,5)
                .HasColumnOrder(13); 

        }

    }
}
