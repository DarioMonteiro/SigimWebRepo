using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Comercial;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.CredCob
{
    public class TituloDetalheCredCob : BaseEntity
    {
        public int ContratoId { get; set; }
        public ContratoComercial Contrato { get; set; }
        public byte Classe { get; set; }
        public string Situacao { get; set; }
        public string Tipo { get; set; }
        public string Tipologia { get; set; }
        public Nullable<DateTime> DataCancelamento { get; set; }
        public Nullable<DateTime> DataDesdobramento { get; set; }
        public Nullable<DateTime> DataEmissaoCobranca { get; set; }
        public DateTime DataVencimento { get; set; }
        public Nullable<DateTime> DataPagamento { get; set; }
        public string MotivoBaixa { get; set; }
        public int? NumeroAgrupamentoId { get; set; }
        public int? RescisaoId { get; set; }
        public Periodicidade Periodicidade { get; set; }
        public Decimal QtdIndice { get; set; }
        public Nullable<Decimal> QtdIndiceAmortizacao { get; set; }
        public Nullable<Decimal> QtdIndiceJuros { get; set; }
        public Nullable<Decimal> QtdIndiceJurosOriginal { get; set; }
        public Nullable<Decimal> QtdIndiceOriginal { get; set; }
        public Decimal QtdIndiceAmortizacaoOriginal { get; set; }
        public int? ContaCorrenteId { get; set; }
        public int? IndiceId { get; set; }
        public IndiceFinanceiro Indice { get; set; }
        public int? Serie { get; set; }
        public string NumeroParcela { get; set; }
        public int? VerbaCobrancaId { get; set; }
        public VerbaCobranca VerbaCobranca { get; set; }
        public string SistemaOrigem { get; set; }
        public string TipoBaixa { get; set; }
        public string TipoCancelamento { get; set; }
        public int? TituloPrincipal { get; set; }
        public int? TituloPrincipalAgrupamento { get; set; }
        public Nullable<Decimal> ValorBaixa { get; set; }
        public Nullable<Decimal> ValorDesconto { get; set; }
        public Nullable<Decimal> ValorDescontoAntecipacao { get; set; }
        public Nullable<Decimal> ValorDiferencaBaixa { get; set; }
        public Nullable<Decimal> ValorIndiceOriginal { get; set; }
        public Decimal ValorIndiceBase { get; set; }
        public Nullable<Decimal> ValorIndicePagamento { get; set; }
        public Nullable<Decimal> ValorCorrecaoAtraso { get; set; }
        public Nullable<Decimal> ValorCorrecaoProrrata { get; set; }
        public Nullable<Decimal> ValorEncargos { get; set; }
        public Nullable<Decimal> ValorMulta { get; set; }
        public Nullable<Decimal> ValorPercentualJuros { get; set; }
        public int? FormaRecebimentoId { get; set; }
        public FormaRecebimento FormaRecebimento { get; set; }
        public Nullable<Int16> QtdDiasAtraso { get; set; }
        public Nullable<Decimal> ValorPresente { get; set; }
        public int? NumeroAgrupamentoRenegociacaoId { get; set; }
        public string NumeroBoleto { get; set; }
        public int? IndiceAtrasoCorrecaoId { get; set; }
        public IndiceFinanceiro IndiceAtrasoCorrecao { get; set; }

        public VendaSerie VendaSerie { get; set; }

        //----------------------------
        public Decimal ValorNominal { get; set; }
        public Decimal ValorDevido { get; set; }
        public Decimal ValorAtualizado { get; set; }
        public Decimal ValorRecebido { get; set; }
        public Decimal ValorVencido { get; set; }
        public Decimal ValorAVencer { get; set; }
        public Decimal ValorVinculado { get; set; }
        public Decimal ValorNaoVinculado { get; set; }
        public Decimal ValorSaldoDevedor { get; set; }
        public Decimal ValorPenalidades { get; set; }
        public Decimal ValorTotalCorrecaoAtraso { get; set; }
        public Decimal PercentualDesconto { get; set; }
        public string Vencimento { get; set; }
        public int QtdTituloRecebido { get; set; }
        public int QtdTituloVencido { get; set; }
        public int QtdTituloAVencer { get; set; }
        public DateTime DataReferencia { get; set; }
        public DateTime DataReferenciaDefasada { get; set; }
        public DateTime DataVencimentoDefasada { get; set; }
        public DateTime DataReferenciaDefasadaAtrasado { get; set; } 
        public DateTime DataVencimentoDefasadaAtrasado { get; set; } 
        public Decimal ValorIndiceDataReferencia { get; set; } 
        public Decimal ValorIndiceDataVencimento { get; set; }
        public Decimal ValorIndiceDataReferenciaDefasada { get; set; }
        public Decimal ValorIndiceDataVencimentoDefasada { get; set; }
        public Decimal ValorTituloDataReferencia { get; set; }
        public Decimal ValorAmortizacaoDataReferencia { get; set; }
        public Decimal ValorJurosDataReferencia { get; set; }
        public Decimal ValorTituloDataBase { get; set; }
        public Decimal ValorAmortizacaoDataBase { get; set; }
        public Decimal ValorJurosDataBase { get; set; }
        public Decimal ValorTituloPresente { get; set; }
        public Decimal ValorTituloPresenteCheio { get; set; }
        public Decimal ValorTituloPresenteProRataDia { get; set; }
        public Decimal ValorTituloAtrasado { get; set; }
        public Decimal PercentualJuros { get; set; }
        public int QtdMesesDefasagemCorrecaoAtraso { get; set; }
        public int QtdDiasProrrata { get; set; }
        public Decimal ValorIndiceDataReferenciaDefasadaAtrasado { get; set; }
        public Decimal ValorIndiceDataVencimentoDefasadaAtrasado { get; set; }
        public Decimal FatorCorrecao { get; set; }
        public Decimal FatorCorrecaoProrrata { get; set; }
        public Decimal ValorTituloDataReferenciaCorrigido { get; set; }
        public Decimal PercentualMultaAtraso { get; set; }
        public Decimal TaxaPermanenciaDiaria { get; set; }
        public Decimal ValorTituloPenalidades { get; set; }
        public Decimal ValorTituloPagoCorrigido { get; set; }
        public Decimal ValorIndiceBaseTemp { get; set; }
        public Decimal QtdIndicePagamento { get; set; }
        public DateTime DataPagamentoDefasada { get; set; } 
        public Decimal ValorIndiceDataPagamentoDefasada { get; set; }
        public Decimal PercentualReceita { get; set; }
        public Decimal ValorTituloOriginal { get; set; }
        public int QtdMesesAtraso { get; set; }
        public Decimal ValorMultaBanco { get; set; }
        public Decimal ValorEncargosBanco { get; set; }
        public Decimal ValorDevidoBanco { get; set; }
        public DateTime DataReferenciaDescapitalizacao { get; set; }
        public int QtdDiasProrrataDescapitalizacao { get; set; }
        public Decimal ValorAmortizacaoOriginal { get; set; }

        public TituloDetalheCredCob(){
            this.PercentualReceita = 100.0m;
        }
    }
}
