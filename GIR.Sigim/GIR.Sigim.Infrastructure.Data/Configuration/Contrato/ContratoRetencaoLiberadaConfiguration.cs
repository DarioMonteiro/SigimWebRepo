using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Contrato
{
    public class ContratoRetencaoLiberadaConfiguration : EntityTypeConfiguration<ContratoRetencaoLiberada>
    {

        public ContratoRetencaoLiberadaConfiguration()
        {
            ToTable("ContratoRetencaoLiberada", "Contrato");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.ContratoRetencaoId)
                .HasColumnName("contratoRetencao")
                .HasColumnOrder(2);

            HasRequired<ContratoRetencao>(l => l.ContratoRetencao)
                .WithMany(c => c.ListaContratoRetencaoLiberada)
                .HasForeignKey(l => l.ContratoRetencaoId);

            Property(l => l.TipoDocumentoId)
                .HasColumnName("tipoDocumento")
                .HasColumnOrder(3);

            HasRequired<TipoDocumento>(l => l.TipoDocumento)
                .WithMany(c => c.ListaContratoRetencaoLiberada)
                .HasForeignKey(l => l.TipoDocumentoId);

            Property(l => l.NumeroDocumento)
                .HasColumnName("numeroDocumento")
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnOrder(4);

            Property(l => l.DataVencimento)
                .HasColumnName("dataVencimento")
                .IsRequired()
                .HasColumnOrder(5);

            Property(l => l.ValorLiberado)
                .HasColumnName("valorLiberado")
                .HasPrecision(18, 5)
                .HasColumnOrder(6);

            Property(l => l.DataLiberacao)
                .HasColumnName("dataLiberacao")
                .IsRequired()
                .HasColumnOrder(7);

            Property(l => l.UsuarioLiberacao)
                .HasColumnName("usuarioLiberacao")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnOrder(8);

            Property(l => l.TipoCompromissoId)
                .HasColumnName("tipoCompromisso")
                .HasColumnOrder(9);

            HasOptional<TipoCompromisso>(l => l.TipoCompromisso)
                .WithMany(c => c.ListaContratoRetencaoLiberada)
                .HasForeignKey(l => l.TipoCompromissoId);

            Property(l => l.TituloPagarId)
                .HasColumnName("tituloPagar")
                .HasColumnOrder(10);

            HasOptional<TituloPagar>(l => l.TituloPagar)
                .WithMany(c => c.ListaContratoRetencaoLiberada)
                .HasForeignKey(l => l.TituloPagarId);

            Property(l => l.TituloReceberId)
                .HasColumnName("tituloReceber")
                .HasColumnOrder(11);

            HasOptional<TituloReceber>(l => l.TituloReceber)
                .WithMany(c => c.ListaContratoRetencaoLiberada)
                .HasForeignKey(l => l.TituloReceberId);
        }
    }
}
