using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.CredCob
{
    public class TituloDetalheCredCobDTO : TituloCredCobDTO
    {
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
        public DateTime dataReferencia { get; set; }
        public DateTime dataReferenciaDefasada { get; set; }
        public DateTime dataVencimentoDefasada { get; set; }
        public DateTime dataReferenciaDefasadaAtrasado { get; set; } 
        public DateTime dataVencimentoDefasadaAtrasado { get; set; } 
        public Decimal valorIndiceDataReferencia { get; set; } 
        public Decimal valorIndiceDataVencimento { get; set; }
        public Decimal valorIndiceDataReferenciaDefasada { get; set; }
        public Decimal valorIndiceDataVencimentoDefasada { get; set; }
        public Decimal valorTituloDataReferencia { get; set; }
        public Decimal valorAmortizacaoDataReferencia { get; set; }
        public Decimal valorJurosDataReferencia { get; set; }
        public Decimal valorTituloDataBase { get; set; }
        public Decimal valorAmortizacaoDataBase { get; set; }
        public Decimal valorJurosDataBase { get; set; }
        public Decimal valorTituloPresente { get; set; }
        public Decimal valorTituloPresenteCheio { get; set; }
        public Decimal valorTituloPresenteProRataDia { get; set; }
        public Decimal valorTituloAtrasado { get; set; }
        public Decimal percentualJuros { get; set; }
        public int qtdMesesDefasagemCorrecaoAtraso { get; set; }
        public int qtdDiasProrrata { get; set; }
        public Decimal valorIndiceDataReferenciaDefasadaAtrasado { get; set; }
        public Decimal valorIndiceDataVencimentoDefasadaAtrasado { get; set; }
        public Decimal fatorCorrecao { get; set; }
        public Decimal fatorCorrecaoProrrata { get; set; }
        public Decimal valorTituloDataReferenciaCorrigido { get; set; }
        public Decimal percentualMultaAtraso { get; set; }
        public Decimal taxaPermanenciaDiaria { get; set; }
        public Decimal valorTituloPenalidades { get; set; }
        public Decimal valorTituloPagoCorrigido { get; set; }
        public Decimal valorIndiceBaseTemp { get; set; }
        public Decimal qtdIndicePagamento { get; set; }
        public DateTime dataPagamentoDefasada { get; set; } 
        public Decimal valorIndiceDataPagamentoDefasada { get; set; }
        public Decimal percentualReceita { get; set; }
        public Decimal valorTituloOriginal { get; set; }
        public int qtdMesesAtraso { get; set; }
        public Decimal valorMultaBanco { get; set; }
        public Decimal valorEncargosBanco { get; set; }
        public Decimal valorDevidoBanco { get; set; }
        public DateTime dataReferenciaDescapitalizacao { get; set; }
        public int qtdDiasProrrataDescapitalizacao { get; set; }
        public Decimal valorAmortizacaoOriginal { get; set; }

        public TituloDetalheCredCobDTO(){
            this.percentualReceita = 100.0m;
        }


    }
}
