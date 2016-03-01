using System;
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


        //public decimal ObterValorDevido(Nullable<DateTime> dataReferencia, bool corrigeParcelaResiduo)
        //{
        //    //var quantidadeTotalMedida = (from med in ListaContratoRetificacaoItemMedicao
        //    //                             where (med.Situacao == SituacaoMedicao.AguardandoAprovacao || med.Situacao == SituacaoMedicao.AguardandoLiberacao) &&
        //    //                                   (med.SequencialItem == sequencialItem && med.SequencialCronograma == sequencialCronograma)
        //    //                             select med.Quantidade).Sum();

        //    decimal valorDevido = 0;
        //    decimal valorTituloAtrasado = 0;
        //    decimal valorTituloDataBase = 0;
        //    //decimal valorMulta = 0;
        //    decimal valorTituloDataReferencia = 0;
        //    decimal valorIndiceDataVencimentoDefasada = 0;
        //    decimal valorIndiceDataReferenciaDefasada = 0;
        //    DateTime dataVencimentoDefasada;
        //    DateTime dataReferenciaDefasada;
        //    DateTime ultimaData;
        //    //decimal fatorCorrecao = 0;

        //    if (dataReferencia.HasValue) {
        //        dataReferencia = dataReferencia.Value.Date;
        //    }

        //    if ((Situacao == "P") || (Situacao == "Q")){
        //        if (!dataReferencia.HasValue) {
        //            dataReferencia = DataVencimento;
        //        }

        //        //Trata dia util
        //        //Trata data referencia
        //        if (dataReferencia.HasValue) {
        //            if (DataVencimento < dataReferencia){
        //                ClienteFornecedor clienteTitular = Contrato.Venda.Contrato.ListaVendaParticipante.Where(l => l.TipoParticipanteId == 1).FirstOrDefault().Cliente;
        //                bool achouProximoDiaUtil = false;
        //                DateTime dataUtil = DataVencimento.AddDays(-1);

        //                while (!achouProximoDiaUtil){
        //                    dataUtil = dataUtil.AddDays(1);
        //                    Feriado feriado = null;
        //                    if (clienteTitular.Correspondencia == "R"){
        //                        feriado = clienteTitular.EnderecoResidencial.UnidadeFederacao.ListaFeriado.Where(l => l.Data.Value.Date == dataUtil.Date).FirstOrDefault(); 
        //                    }
        //                    if (clienteTitular.Correspondencia == "C"){
        //                        feriado = clienteTitular.EnderecoComercial.UnidadeFederacao.ListaFeriado.Where(l => l.Data.Value.Date == dataUtil.Date).FirstOrDefault(); 
        //                    }
        //                    if (clienteTitular.Correspondencia == "O"){
        //                        feriado = clienteTitular.EnderecoOutro.UnidadeFederacao.ListaFeriado.Where(l => l.Data.Value.Date == dataUtil.Date).FirstOrDefault(); 
        //                    }
        //                    if (feriado == null){
        //                        if ((dataUtil.DayOfWeek != DayOfWeek.Saturday)&&(dataUtil.DayOfWeek != DayOfWeek.Sunday)){
        //                            achouProximoDiaUtil = true;
        //                        }
        //                    }
        //                }
        //                if (dataUtil >= dataReferencia.Value){
        //                    dataReferencia = DataVencimento;
        //                }
        //            }
        //        }
        //        //Trata data referencia

        //    }

        //    if (Situacao == "P"){

        //        //VendaSerie vendaSerie = Contrato.ListaVendaSerie.Where(l => l.NumeroSerie == Serie).FirstOrDefault();

        //        CotacaoValores cotacaoValores = null;
        //        //Calcula cotacao 
        //        dataReferenciaDefasada = dataReferencia.Value.Date.AddMonths(VendaSerie.DefasagemMesIndiceCorrecao * -1);
        //        ultimaData = Indice.ListaCotacaoValores.Where(l => l.Data <= dataReferenciaDefasada).Select(l => l.Data.Value).Max();
        //        cotacaoValores = Indice.ListaCotacaoValores.Where(l => l.Data == ultimaData).FirstOrDefault();
        //        //Fim calcula cotacao

        //        valorIndiceDataReferenciaDefasada = cotacaoValores.Valor.HasValue ? cotacaoValores.Valor.Value : 0;


        //        //Calcula cotacao 
        //        dataVencimentoDefasada = DataVencimento.Date.AddMonths(VendaSerie.DefasagemMesIndiceCorrecao * -1);
        //        ultimaData = Indice.ListaCotacaoValores.Where(l => l.Data <= dataVencimentoDefasada).Select(l => l.Data.Value).Max();
        //        cotacaoValores = Indice.ListaCotacaoValores.Where(l => l.Data == ultimaData).FirstOrDefault();
        //        //Fim calcula cotacao

        //        valorIndiceDataVencimentoDefasada = cotacaoValores.Valor.HasValue ? cotacaoValores.Valor.Value : 0;

        //        //Calcula valor atualizado
        //        if (dataReferencia >= DataVencimento){
        //            valorTituloDataReferencia = QtdIndice * valorIndiceDataVencimentoDefasada;
        //        }
        //        else {
        //            valorTituloDataReferencia = QtdIndice * valorIndiceDataReferenciaDefasada;
        //        }
        //        //Calcula valor atualizado

        //        if (!corrigeParcelaResiduo){
        //            if ((Situacao == "P") && (VendaSerie.CobrancaResiduo =="S")){
        //                valorTituloDataReferencia = QtdIndice * ValorIndiceBase;
        //            }
        //        }

        //        if (corrigeParcelaResiduo){
        //            if ((Situacao == "P") && (VendaSerie.CobrancaResiduo =="S") && (dataReferencia.Value > DataVencimento)){
        //                valorTituloDataReferencia = QtdIndice * ValorIndiceBase;
        //            }
        //        }

        //        valorTituloDataBase = valorTituloDataReferencia;

        //        if (!corrigeParcelaResiduo){
        //            if ((Situacao == "P") && (VendaSerie.CobrancaResiduo =="S")){
        //                valorTituloDataBase = QtdIndice * ValorIndiceBase;
        //            }
        //        }

        //        if (corrigeParcelaResiduo){
        //            if ((Situacao == "P") && (VendaSerie.CobrancaResiduo =="S") && (dataReferencia.Value > DataVencimento)){
        //                valorTituloDataBase = QtdIndice * ValorIndiceBase;
        //            }
        //        }

        //        valorTituloAtrasado = valorTituloDataBase;


        //        if (DataVencimento < dataReferencia)
        //        {
        //            valorTituloDataReferenciaCorrigido = valorTituloDataReferencia;

        //            if (IndiceAtrasoCorrecaoId > 1){
        //                CotacaoValores cotacaoIndiceAtrasoDtRef = null;
        //                //Calcula cotacao 
        //                ultimaData = IndiceAtrasoCorrecao.ListaCotacaoValores.Where(l => l.Data <= dataReferenciaDefasada).Select(l => l.Data.Value).Max();
        //                cotacaoIndiceAtrasoDtRef = IndiceAtrasoCorrecao.ListaCotacaoValores.Where(l => l.Data == ultimaData).FirstOrDefault();
        //                //Fim calcula cotacao

        //                CotacaoValores cotacaoIndiceAtrasoDtVencto = null;
        //                //Calcula cotacao 
        //                ultimaData = IndiceAtrasoCorrecao.ListaCotacaoValores.Where(l => l.Data <= dataVencimentoDefasada).Select(l => l.Data.Value).Max();
        //                cotacaoIndiceAtrasoDtVencto = IndiceAtrasoCorrecao.ListaCotacaoValores.Where(l => l.Data == ultimaData).FirstOrDefault();
        //                //Fim calcula cotacao


        //                if ((cotacaoIndiceAtrasoDtRef.Valor.HasValue) && (cotacaoIndiceAtrasoDtVencto.Valor.HasValue))
        //                {
        //                    fatorCorrecao = (cotacaoIndiceAtrasoDtRef.Valor.Value / cotacaoIndiceAtrasoDtVencto.Valor.Value) -1;
        //                }

        //                if ((cotacaoIndiceAtrasoDtRef.Valor.HasValue) && (cotacaoIndiceAtrasoDtVencto.Valor.HasValue))
        //                {
        //                    if (DataVencimento.Day > dataReferencia.Value.Day){
        //                        //Calcula cotacao 
        //                        ultimaData = IndiceAtrasoCorrecao.ListaCotacaoValores.Where(l => l.Data <= dataReferenciaDefasada.AddMonths(-1)).Select(l => l.Data.Value).Max();
        //                        cotacaoIndiceAtrasoDtRef = IndiceAtrasoCorrecao.ListaCotacaoValores.Where(l => l.Data == ultimaData).FirstOrDefault();
        //                        //Fim calcula cotacao


        //                        fatorCorrecao = (cotacaoIndiceAtrasoDtRef.Valor.Value / cotacaoIndiceAtrasoDtVencto.Valor.Value) -1;
        //                        }
        //                }


        //                valorTituloDataReferenciaCorrigido = valorTituloDataReferencia + (valorTituloDataReferencia + fatorCorrecao);

        //                valorCorrecaoAtraso = valorTituloDataReferenciaCorrigido - valorTituloDataReferencia;
        //            }

        //            valorCorrecaoAtraso = valorTituloDataReferenciaCorrigido - valorTituloDataReferencia;


        //            if ((Contrato.Unidade.ConsiderarParametroUnidade.HasValue) && (Contrato.Unidade.ConsiderarParametroUnidade.Value))
        //            {
        //                Decimal percentualMultaPorAtraso = Contrato.Unidade.MultaPorAtraso.HasValue ? Contrato.Unidade.MultaPorAtraso.Value : 0;
        //                ValorMulta = ((valorTituloDataBase + valorCorrecaoAtraso + AUX.valorCorrecaoProrrata) * (percentualMultaPorAtraso / 100.0m));
        //            }
        //        }


        //        if (DataVencimento < dataReferencia){
        //            valorTituloAtrasado = valorTituloDataBase + valorMulta + valorEncargos + valorCorrecaoAtraso + valorCorrecaoProrrata
        //        }
        //        valorDevido = valorTituloAtrasado;
        //    }

        //    return valorDevido;
        //}

    }
}
