using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Comercial;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Comercial
{
    public class RenegociacaoConfiguration : EntityTypeConfiguration<Renegociacao>
    {

        public RenegociacaoConfiguration()
        {
            ToTable("Renegociacao", "Comercial");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.ContratoId)
                .HasColumnName("contrato")
                .HasColumnOrder(2);

            HasRequired<ContratoComercial>(l => l.Contrato)
                .WithMany(c => c.ListaRenegociacao)
                .HasForeignKey(l => l.ContratoId);

            Property(l => l.DataReferencia)
                .IsRequired()
                .HasColumnName("dataReferencia")
                .HasColumnOrder(3);

            Property(l => l.DataRenegociacao)
                .IsRequired()
                .HasColumnName("dataRenegociacao")
                .HasColumnOrder(4);

            Property(l => l.ValorRenegociado)
                .IsRequired()
                .HasColumnName("valorRenegociado")
                .HasPrecision(18,2)
                .HasColumnOrder(5);

            Property(l => l.DataCancelamento)
                .HasColumnName("dataCancelamento")
                .HasColumnOrder(6);

            Property(l => l.MotivoCancelamento)
                .HasColumnName("motivoCancelamento")
                .HasMaxLength(200)
                .HasColumnOrder(7);

            Property(l => l.DataCadastramento)
                .HasColumnName("dataCadastramento")
                .HasColumnOrder(8);

            Property(l => l.Tipo)
                .HasColumnName("tipo")
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnOrder(9);

            Property(l => l.Aprovado)
                .HasColumnName("aprovado")
                .HasColumnType("bit")
                .HasColumnOrder(10);

            Property(l => l.DataAprovacao)
                .HasColumnName("dataAprovacao")
                .HasColumnOrder(11);

            Property(l => l.UsuarioAprovacao)
                .HasColumnName("usuarioAprovacao")
                .HasMaxLength(50)
                .HasColumnOrder(12);

        }

    }
}
