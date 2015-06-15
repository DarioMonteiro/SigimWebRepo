using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;  
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Contrato
{
    public class ContratoRetificacaoConfiguration : EntityTypeConfiguration<ContratoRetificacao>
    {
        public ContratoRetificacaoConfiguration()
        {
            ToTable("ContratoRetificacao", "Contrato");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.ContratoId)
                .IsRequired() 
                .HasColumnName("contrato")
                .HasColumnOrder(2);

            HasRequired<Domain.Entity.Contrato.Contrato>(l => l.Contrato)
                .WithMany(c => c.ListaContratoRetificacao)
                .HasForeignKey(l => l.ContratoId);

            Property(l => l.Sequencial)
                .IsRequired() 
                .HasColumnName("sequencial")
                .HasColumnOrder(3);

            Property(l => l.Aprovada)
                .IsRequired()
                .HasColumnType("bit")
                .HasColumnName("aprovada")
                .HasColumnOrder(4);

            Property(l => l.DataAprovacao)
                .HasColumnName("dataAprovacao")
                .HasColumnOrder(5);

            Property(l => l.UsuarioAprovacao)
                .HasColumnName("usuarioAprovacao")
                .HasMaxLength(50)
                .HasColumnOrder(6);

            Property(l => l.Motivo)
                .HasColumnName("motivo")
                .HasMaxLength(4000)
                .HasColumnOrder(7);

            Property(l => l.Observacao)
                .HasColumnName("observacao")
                .HasMaxLength(4000)
                .HasColumnOrder(8);

            Property(l => l.Anotacoes)
                .HasColumnName("anotacoes")
                .HasMaxLength(4000)
                .HasColumnOrder(9);

            Property(l => l.ReferenciaDigital)
                .HasColumnName("referenciaDigital")
                .HasMaxLength(255)
                .HasColumnOrder(10);

            Property(l => l.RetencaoContratual)
                .HasColumnName("retencaoContratual")
                .HasPrecision(18, 5)
                .HasColumnOrder(11);

            Property(l => l.RetencaoPrazoResgate)
                .HasColumnName("retencaoPrazoResgate")
                .HasColumnOrder(12);

            Property(l => l.RetencaoTipoCompromissoId)
                .HasColumnName("retencaoTipoCompromisso")
                .HasColumnOrder(13);

            HasOptional<TipoCompromisso>(l => l.RetencaoTipoCompromisso)
                .WithMany(c => c.ListaContratoRetificacao)
                .HasForeignKey(l => l.RetencaoTipoCompromissoId);

            HasMany<ContratoRetificacaoItem>(l => l.ListaContratoRetificacaoItem)
                .WithRequired(c => c.ContratoRetificacao)
                .HasForeignKey(c => c.ContratoRetificacaoId);

            HasMany<ContratoRetificacaoProvisao>(l => l.ListaContratoRetificacaoProvisao)
                .WithRequired(c => c.ContratoRetificacao)
                .HasForeignKey(c => c.ContratoRetificacaoId);

            HasMany<ContratoRetificacaoItemCronograma>(l => l.ListaContratoRetificacaoItemCronograma)
                .WithRequired(c => c.ContratoRetificacao)
                .HasForeignKey(c => c.ContratoRetificacaoId);

            HasMany<ContratoRetificacaoItemMedicao>(l => l.ListaContratoRetificacaoItemMedicao)
                .WithRequired(c => c.ContratoRetificacao)
                .HasForeignKey(c => c.ContratoRetificacaoId);

            HasMany<ContratoRetificacaoItemImposto>(l => l.ListaContratoRetificacaoItemImposto)
                .WithRequired(c => c.ContratoRetificacao)
                .HasForeignKey(c => c.ContratoRetificacaoId);
        }
    }
}
