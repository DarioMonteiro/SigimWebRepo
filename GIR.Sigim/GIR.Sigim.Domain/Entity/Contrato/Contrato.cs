using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro; 
using GIR.Sigim.Domain.Entity.Sigim;   

namespace GIR.Sigim.Domain.Entity.Contrato
{
    public class Contrato : BaseEntity
    {
        public string CodigoCentroCusto { get; set; }
        public CentroCusto CentroCusto { get; set; }
        public int? LicitacaoId { get; set; }
        public Licitacao Licitacao { get; set; }
        public int ContratanteId { get; set; }
        public ClienteFornecedor Contratante { get; set; }
        public int ContratadoId { get; set; }
        public ClienteFornecedor Contratado { get; set; }
        public int? IntervenienteId { get; set; } 
        public ClienteFornecedor Interveniente { get; set; }
        public int ContratoDescricaoId { get; set; }
        public LicitacaoDescricao ContratoDescricao { get; set; }
        public SituacaoContrato Situacao { get; set; }
        public Nullable<DateTime> DataAssinatura { get; set; }
        public string DocumentoOrigem { get; set; }
        public string NumeroEmpenho { get; set; }
        public decimal? ValorContrato { get; set; }
        public DateTime DataCadastro { get; set; }
        public string UsuarioCadastro { get; set; }
        public Nullable<DateTime> DataCancela { get; set; }
        public string UsuarioCancela { get; set; }
        public string MotivoCancela { get; set; }
        public int TipoContrato { get; set; }


        //************************************************************************************************
        public decimal ObterQuantidadeTotalMedida(int? sequencialItem, int? sequencialCronograma)
        {
            var quantidadeTotalMedida = (from med in ListaContratoRetificacaoItemMedicao
                                         where (med.Situacao == SituacaoMedicao.AguardandoAprovacao || med.Situacao == SituacaoMedicao.AguardandoLiberacao) &&
                                               (med.SequencialItem == sequencialItem && med.SequencialCronograma == sequencialCronograma)
                                         select med.Quantidade).Sum();

            return quantidadeTotalMedida;
        }


        ////(SELECT ISNULL(SUM(AUX.valor), 0) FROM Contrato.contratoRetificacaoItemMedicao AUX
        //// INNER JOIN Contrato.ContratoRetificacaoItem			AUX2	ON AUX2.codigo = MED.contratoRetificacaoItem	
        //// INNER JOIN Contrato.ContratoRetificacaoItemCronograma	AUX3	ON AUX3.codigo = MED.contratoRetificacaoItemCronograma
        //// WHERE (AUX.sequencialItem = AUX2.sequencial) AND (AUX.sequencialCronograma = AUX3.sequencial) 
        //// AND (AUX.Contrato = MED.contrato) AND (AUX.situacao IN (0,1))) AS valorTotalMedido,

        //public decimal ValorTotalMedido
        //{
        //    get
        //    {
        //        return ListaContratoRetificacaoItemMedicao
        //            .Where(l => l.Situacao == SituacaoMedicao.AguardandoAprovacao || l.Situacao == SituacaoMedicao.AguardandoLiberacao)
        //            .Where(l => ListaContratoRetificacaoItem.Any(c => c.Sequencial == l.SequencialItem))
        //            .Where(l => ListaContratoRetificacaoItemMedicao.Any(c => c.ContratoRetificacaoItemCronograma.Sequencial == l.SequencialCronograma))
        //            .Sum(l => l.Valor);
        //    }
        //}

        public decimal ObterValorTotalMedido(int? sequencialItem, int? sequencialCronograma) {
            var valorTotalMedido = (from med in ListaContratoRetificacaoItemMedicao
                                     //join item in ListaContratoRetificacaoItem on med.SequencialItem equals item.Sequencial
                                     //join cron in ListaContratoRetificacaoItemCronograma on med.SequencialCronograma equals cron.Sequencial
                                    where (med.Situacao == SituacaoMedicao.AguardandoAprovacao || med.Situacao == SituacaoMedicao.AguardandoLiberacao) &&
                                          (med.SequencialItem == sequencialItem &&  med.SequencialCronograma == sequencialCronograma)
                                        //&& (item.Sequencial == sequencialItem 
                                        //&&  cron.Sequencial == sequencialCronograma)
                                    select med.Valor).Sum();

            return valorTotalMedido;
        }

        public decimal ObterQuantidadeTotalLiberada(int? sequencialItem, int? sequencialCronograma)
        {
            var quantidadeTotalLiberada = (from med in ListaContratoRetificacaoItemMedicao
                                           where med.Situacao == SituacaoMedicao.Liberado &&
                                                (med.SequencialItem == sequencialItem && med.SequencialCronograma == sequencialCronograma)
                                           select med.Quantidade).Sum();

            return quantidadeTotalLiberada;
        }

        public decimal ObterValorTotalLiberado(int? sequencialItem, int? sequencialCronograma)
        {
            var valorTotalLiberado = (from med in ListaContratoRetificacaoItemMedicao
                                      where med.Situacao == SituacaoMedicao.Liberado &&
                                           (med.SequencialItem == sequencialItem && med.SequencialCronograma == sequencialCronograma)
                                      select med.Valor).Sum();

            return valorTotalLiberado;
        }

        public decimal ObterQuantidadeTotalMedidaLiberada(int? sequencialItem, int? sequencialCronograma)
        {
            var quantidadeTotalMedidaLiberada = 
                                        (from med in ListaContratoRetificacaoItemMedicao
                                         where (med.Situacao == SituacaoMedicao.AguardandoAprovacao || med.Situacao == SituacaoMedicao.AguardandoLiberacao || med.Situacao == SituacaoMedicao.Liberado) &&
                                               (med.SequencialItem == sequencialItem && med.SequencialCronograma == sequencialCronograma)
                                         select med.Quantidade).Sum();

            return quantidadeTotalMedidaLiberada;
        }

        public decimal ObterValorTotalMedidoLiberado(int? sequencialItem, int? sequencialCronograma)
        {
            var valorTotalMedidoLiberado = (from med in ListaContratoRetificacaoItemMedicao
                                            where (med.Situacao == SituacaoMedicao.AguardandoAprovacao || med.Situacao == SituacaoMedicao.AguardandoLiberacao || med.Situacao == SituacaoMedicao.Liberado) &&
                                                  (med.SequencialItem == sequencialItem && med.SequencialCronograma == sequencialCronograma)
                                            select med.Valor).Sum();

            return valorTotalMedidoLiberado;
        }

        public decimal ObterValorTotalImpostoRetido(int? sequencialItem, 
                                                    int? sequencialCronograma, 
                                                    int? contratoRetificacaoItemId)
        {
            //var teste = (from med in ListaContratoRetificacaoItemMedicao
            //             join itemImposto in ListaContratoRetificacaoItemImposto on med.ContratoRetificacaoItemId equals itemImposto.ContratoRetificacaoItemId
            //             where (itemImposto.ImpostoFinanceiro.Retido == true &&
            //                    med.SequencialItem == sequencialItem &&
            //                    med.SequencialCronograma == sequencialCronograma &&
            //                    med.ContratoRetificacaoItemId == contratoRetificacaoItemId)
            //             select (((med.Valor * itemImposto.PercentualBaseCalculo) / 100) * itemImposto.ImpostoFinanceiro.Aliquota) / 100).ToList();

            var valorTotalImpostoRetido = (from med in ListaContratoRetificacaoItemMedicao
                                            join itemImposto in ListaContratoRetificacaoItemImposto on med.ContratoRetificacaoItemId equals itemImposto.ContratoRetificacaoItemId
                                           where (itemImposto.ImpostoFinanceiro.Retido == true) &&
                                                  ( med.SequencialItem == sequencialItem && 
                                                    med.SequencialCronograma == sequencialCronograma &&
                                                    med.ContratoRetificacaoItemId == contratoRetificacaoItemId) 
                                           select ((((med.Valor * itemImposto.PercentualBaseCalculo) / 100) * itemImposto.ImpostoFinanceiro.Aliquota) / 100)).Sum();

            return valorTotalImpostoRetido;
        }

        public decimal ObterValorTotalImpostoRetidoMedicao(int? sequencialItem,
                                                           int? sequencialCronograma,
                                                           int? contratoRetificacaoItemId,
                                                           int? id)
        {
            var valorTotalImpostoRetidoMedicao = (from med in ListaContratoRetificacaoItemMedicao
                                                  join itemImposto in ListaContratoRetificacaoItemImposto on med.ContratoRetificacaoItemId equals itemImposto.ContratoRetificacaoItemId
                                                  where (itemImposto.ImpostoFinanceiro.Retido == true) &&
                                                        (med.SequencialItem == sequencialItem &&
                                                        med.SequencialCronograma == sequencialCronograma &&
                                                        med.ContratoRetificacaoItemId == contratoRetificacaoItemId &&
                                                        med.Id == id)
                                                  select ((((med.Valor * itemImposto.PercentualBaseCalculo) / 100) * itemImposto.ImpostoFinanceiro.Aliquota) / 100)).Sum();

            return valorTotalImpostoRetidoMedicao;
        }

        public decimal ObterValorTotalImpostoIndiretoMedicao(int? sequencialItem,
                                                             int? sequencialCronograma,
                                                             int? contratoRetificacaoItemId,
                                                             int? id)
        {
            var valorTotalImpostoRetidoMedicao = (from med in ListaContratoRetificacaoItemMedicao
                                                  join itemImposto in ListaContratoRetificacaoItemImposto on med.ContratoRetificacaoItemId equals itemImposto.ContratoRetificacaoItemId
                                                  where (itemImposto.ImpostoFinanceiro.Indireto == true) &&
                                                        (med.SequencialItem == sequencialItem &&
                                                        med.SequencialCronograma == sequencialCronograma &&
                                                        med.ContratoRetificacaoItemId == contratoRetificacaoItemId &&
                                                        med.Id == id)
                                                  select ((((med.Valor * itemImposto.PercentualBaseCalculo) / 100) * itemImposto.ImpostoFinanceiro.Aliquota) / 100)).Sum();

            return valorTotalImpostoRetidoMedicao;
        }

        public decimal ObterValorTotalMedidoIndireto(int? contratoId,
                                                     string numeroDocumento,
                                                     int? tipoDocumentoId,
                                                     Nullable<DateTime> dataVencimento)
        {
            var valorTotalMedidoIndireto = (from med in ListaContratoRetificacaoItemMedicao
                                                join itemImposto in ListaContratoRetificacaoItemImposto on med.ContratoRetificacaoItemId equals itemImposto.ContratoRetificacaoItemId
                                            where (itemImposto.ImpostoFinanceiro.Indireto == true) &&
                                                  (med.ContratoId == contratoId &&
                                                   med.TipoDocumentoId == tipoDocumentoId &&
                                                   med.NumeroDocumento == numeroDocumento &&
                                                   med.DataVencimento == dataVencimento)
                                            select med.Valor).Sum();

            return valorTotalMedidoIndireto;
        }

        public decimal ObterValorTotalMedidoNota(int? contratoId,
                                                 string numeroDocumento,
                                                 int? tipoDocumentoId,
                                                 Nullable<DateTime> dataVencimento)
        {
            var valorTotalTotalMedidoNota = (from med in ListaContratoRetificacaoItemMedicao
                                             where (med.ContratoId == contratoId &&
                                                    med.TipoDocumentoId == tipoDocumentoId &&
                                                    med.NumeroDocumento == numeroDocumento &&
                                                    med.DataVencimento == dataVencimento)
                                             select med.Valor).Sum();

            return valorTotalTotalMedidoNota;
        }

        //***************************************************************************************************

        public ICollection<ContratoRetificacao> ListaContratoRetificacao { get; set; }
        public ICollection<ContratoRetificacaoItem> ListaContratoRetificacaoItem { get; set; }
        public ICollection<ContratoRetificacaoItemMedicao> ListaContratoRetificacaoItemMedicao { get; set; }
        public ICollection<ContratoRetificacaoItemCronograma> ListaContratoRetificacaoItemCronograma { get; set; }
        public ICollection<ContratoRetificacaoItemImposto> ListaContratoRetificacaoItemImposto { get; set; }
        public ICollection<ContratoRetificacaoProvisao> ListaContratoRetificacaoProvisao { get; set; }

        public Contrato()
        {
            this.Situacao = SituacaoContrato.Minuta;
            this.ListaContratoRetificacao = new HashSet<ContratoRetificacao>();
            this.ListaContratoRetificacaoItem = new HashSet<ContratoRetificacaoItem>(); 
            this.ListaContratoRetificacaoItemMedicao = new HashSet<ContratoRetificacaoItemMedicao>();
            this.ListaContratoRetificacaoItemCronograma = new HashSet<ContratoRetificacaoItemCronograma>();
            this.ListaContratoRetificacaoItemImposto = new HashSet<ContratoRetificacaoItemImposto>();
            this.ListaContratoRetificacaoProvisao = new HashSet<ContratoRetificacaoProvisao>();
        }

    }
}
