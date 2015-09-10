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

            Property(l => l.Situacao)
                .HasColumnName("situacao")
                .HasColumnType("smallint");
                //.HasColumnOrder(5);

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

            Property(l => l.DataEmissao)
                .HasColumnName("dataEmissao");

            Property(l => l.DataPagamento)
                .HasColumnName("dataPagamento");

            Property(l => l.DataBaixa)
                .HasColumnName("dataBaixa");

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

            Property(l => l.FormaPagamento)
                .HasColumnName("formaPagamento");

            Property(l => l.CodigoInterface)
                .HasColumnName("codigoInterface");

            Property(l => l.SistemaOrigem)
                .HasMaxLength(5)
                .HasColumnType("nchar")
                .HasColumnName("sistemaOrigem");

            Property(l => l.CBBanco)
                .HasMaxLength(3)
                .HasColumnName("cbBanco");

            Property(l => l.CBMoeda)
                .HasMaxLength(1)
                .HasColumnName("cbMoeda");

            Property(l => l.CBCampoLivre)
                .HasMaxLength(25)
                .HasColumnName("cbCampoLivre");

            Property(l => l.CBDV)
                .HasMaxLength(1)
                .HasColumnName("cbDV");

            Property(l => l.CBValor)
                .HasMaxLength(10)
                .HasColumnName("cbValor");

            Property(l => l.CBDataValor)
                .HasMaxLength(4)
                .HasColumnName("cbDataValor");

            Property(l => l.CBBarra)
                .HasColumnName("cbBarra");

            Property(l => l.ValorPago)
                .HasPrecision(18, 5)
                .HasColumnName("valorPago");

            Property(l => l.MotivoCancelamentoId)
                .HasColumnName("motivoCancelamento");

            Property(l => l.MovimentoId)
                .HasColumnName("movimento");

            Property(l => l.BancoBaseBordero)
                .HasColumnName("bancoBaseBordero");

            Property(l => l.AgenciaContaBaseBordero)
                .HasColumnName("agenciaContaBaseBordero");

            Property(l => l.ContaCorrenteId)
                .HasColumnName("contaCorrente");

            Property(l => l.Retencao)
                .HasPrecision(18, 5)
                .HasColumnName("retencao");

            Property(l => l.Observacao)
                .HasMaxLength(100)
                .HasColumnName("observacao");

            Property(l => l.MotivoCancelamentoInterface)
                .HasMaxLength(100)
                .HasColumnName("motivoCancelamentoInterface");

            Property(l => l.EhPagamentoAntecipado)
                .HasColumnName("pagamentoAntecipado");

            Property(l => l.CBConcessionaria)
                .HasMaxLength(48)
                .HasColumnName("cbConcessionaria");

            Property(l => l.TituloPrincipalAgrupamentoId)
                .HasColumnName("tituloPrincipalAgrupamento");

            HasMany<ContratoRetencaoLiberada>(l => l.ListaContratoRetencaoLiberada)
                .WithOptional(c => c.TituloPagar)
                .HasForeignKey(c => c.TituloPagarId);

        }
    }
}