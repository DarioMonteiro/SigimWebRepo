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

            Property(l => l.DataEmissaoDocumento)
                .HasColumnName("dataEmissaoDocumento");

            Property(l => l.TituloPaiId)
                .HasColumnName("tituloPai");

            HasOptional(l => l.TituloPai)
                .WithMany(l => l.ListaFilhos)
                .HasForeignKey(l => l.TituloPaiId);

            Property(l => l.Parcela)
                .HasColumnName("parcela");

            Property(l => l.ValorImposto)
                .HasPrecision(18, 5)
                .HasColumnName("valorImposto");

            Property(l => l.Desconto)
                .HasPrecision(18, 5)
                .HasColumnName("desconto");

            Property(l => l.DataLimiteDesconto)
                .HasColumnName("dataLimiteDesconto");

            Property(l => l.Multa)
                .HasPrecision(18, 5)
                .HasColumnName("multa");

            Property(l => l.EhMultaPercentual)
                .HasColumnName("percentualMulta");

            Property(l => l.TaxaPermanencia)
                .HasPrecision(18, 5)
                .HasColumnName("taxaPermanencia");

            Property(l => l.EhTaxaPermanenciaPercentual)
                .HasColumnName("percentualTaxa");

            Property(l => l.MotivoDesconto)
                .HasMaxLength(30)
                .HasColumnName("motivoDesconto");

            Property(l => l.DataRecebimento)
                .HasColumnName("dataRecebimento");

            Property(l => l.ValorRecebido)
                .HasPrecision(18, 2)
                .HasColumnName("valorRecebido");

            Property(l => l.LoginUsuarioCadastro)
                .HasMaxLength(50)
                .HasColumnName("operadorCadastro");

            Property(l => l.DataCadastro)
                .HasColumnName("dataCadastro");

            Property(l => l.LoginUsuarioSituacao)
                .HasMaxLength(50)
                .HasColumnName("operadorStatus");

            Property(l => l.DataSituacao)
                .HasColumnName("dataStatus");

            Property(l => l.LoginUsuarioApropriacao)
                .HasMaxLength(50)
                .HasColumnName("operadorApropriacao");

            Property(l => l.DataApropriacao)
                .HasColumnName("dataApropriacao");

            Property(l => l.MotivoCancelamentoId)
                .HasColumnName("motivoCancelamento");

            Property(l => l.DataBaixa)
                .HasColumnName("dataBaixa");

            Property(l => l.SistemaOrigem)
                .HasMaxLength(5)
                .HasColumnType("varchar")
                .HasColumnName("sistemaOrigem");

            HasOptional(l => l.MotivoCancelamento)
                .WithMany(c => c.ListaTituloReceber)
                .HasForeignKey(l => l.MotivoCancelamentoId);

            Property(l => l.Observacao)
                .HasMaxLength(100)
                .HasColumnType("varchar")
                .HasColumnName("observacao");

            Property(l => l.Retencao)
                .HasPrecision(18, 5)
                .HasColumnName("retencao");

            HasMany<ContratoRetificacaoItemMedicao>(l => l.ListaContratoRetificacaoItemMedicao)
                .WithOptional(l => l.TituloReceber)
                .HasForeignKey(c => c.TituloReceberId);

            HasMany<ContratoRetencaoLiberada>(l => l.ListaContratoRetencaoLiberada)
                .WithOptional(c => c.TituloReceber)
                .HasForeignKey(c => c.TituloReceberId);

            HasMany<ImpostoReceber>(l => l.ListaImpostoReceber)
                .WithRequired(l => l.TituloReceber)
                .HasForeignKey(c => c.TituloReceberId);
        }
    }
}
