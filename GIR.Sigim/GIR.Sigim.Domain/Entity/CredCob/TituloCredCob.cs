﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Comercial;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.CredCob
{
    public class TituloCredCob : BaseEntity
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

        public ICollection<TituloMovimento> ListaTituloMovimento { get; set; }

        public TituloCredCob()
        {
            this.ListaTituloMovimento = new HashSet<TituloMovimento>();
        }
    }
}
