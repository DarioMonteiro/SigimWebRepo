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
        public TipoContrato TipoContrato { get; set; }
        public ICollection<ContratoRetificacao> ListaContratoRetificacao { get; set; }
        public ICollection<ContratoRetificacaoItem> ListaContratoRetificacaoItem { get; set; }
        public ICollection<ContratoRetificacaoItemMedicao> ListaContratoRetificacaoItemMedicao { get; set; }
        public ICollection<ContratoRetificacaoItemCronograma> ListaContratoRetificacaoItemCronograma { get; set; }
        public ICollection<ContratoRetificacaoItemImposto> ListaContratoRetificacaoItemImposto { get; set; }
        public ICollection<ContratoRetificacaoProvisao> ListaContratoRetificacaoProvisao { get; set; }
        public ICollection<ContratoRetencao> ListaContratoRetencao { get; set; }
        public ICollection<AvaliacaoFornecedor> ListaAvaliacaoFornecedor { get; set; }

        public Contrato()
        {
            this.Situacao = SituacaoContrato.Minuta;
            this.ListaContratoRetificacao = new HashSet<ContratoRetificacao>();
            this.ListaContratoRetificacaoItem = new HashSet<ContratoRetificacaoItem>();
            this.ListaContratoRetificacaoItemMedicao = new HashSet<ContratoRetificacaoItemMedicao>();
            this.ListaContratoRetificacaoItemCronograma = new HashSet<ContratoRetificacaoItemCronograma>();
            this.ListaContratoRetificacaoItemImposto = new HashSet<ContratoRetificacaoItemImposto>();
            this.ListaContratoRetificacaoProvisao = new HashSet<ContratoRetificacaoProvisao>();
            this.ListaContratoRetencao = new HashSet<ContratoRetencao>();
            this.ListaAvaliacaoFornecedor = new HashSet<AvaliacaoFornecedor>();
        }


        public decimal ObterQuantidadeTotalMedida(int? sequencialItem, int? sequencialCronograma)
        {
            var quantidadeTotalMedida = (from med in ListaContratoRetificacaoItemMedicao
                                         where (med.Situacao == SituacaoMedicao.AguardandoAprovacao || med.Situacao == SituacaoMedicao.AguardandoLiberacao) &&
                                               (med.SequencialItem == sequencialItem && med.SequencialCronograma == sequencialCronograma)
                                         select med.Quantidade).Sum();

            return quantidadeTotalMedida;
        }

        public decimal ObterValorTotalMedido(int? sequencialItem, int? sequencialCronograma) {
            var valorTotalMedido = (from med in ListaContratoRetificacaoItemMedicao
                                    where (med.Situacao == SituacaoMedicao.AguardandoAprovacao || med.Situacao == SituacaoMedicao.AguardandoLiberacao) &&
                                          (med.SequencialItem == sequencialItem &&  med.SequencialCronograma == sequencialCronograma)
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

        public decimal ObterValorImpostoRetido(int? sequencialItem, 
                                               int? sequencialCronograma, 
                                               int? contratoRetificacaoItemId)
        {
            var valorImpostoRetido = (from med in ListaContratoRetificacaoItemMedicao
                                      join itemImposto in ListaContratoRetificacaoItemImposto on med.ContratoRetificacaoItemId equals itemImposto.ContratoRetificacaoItemId
                                      where (itemImposto.ImpostoFinanceiro.EhRetido == true) &&
                                            ( med.SequencialItem == sequencialItem && 
                                              med.SequencialCronograma == sequencialCronograma &&
                                              med.ContratoRetificacaoItemId == contratoRetificacaoItemId) 
                                      select ((((med.Valor * itemImposto.PercentualBaseCalculo) / 100) * itemImposto.ImpostoFinanceiro.Aliquota) / 100)).Sum();

            return valorImpostoRetido;
        }

        public decimal ObterValorImpostoRetidoMedicao(int? sequencialItem,
                                                      int? sequencialCronograma,
                                                      int? contratoRetificacaoItemId,
                                                      int? id)
        {
            var valorImpostoRetidoMedicao = (from med in ListaContratoRetificacaoItemMedicao
                                             join itemImposto in ListaContratoRetificacaoItemImposto on med.ContratoRetificacaoItemId equals itemImposto.ContratoRetificacaoItemId
                                             where (itemImposto.ImpostoFinanceiro.EhRetido == true) &&
                                                   (med.SequencialItem == sequencialItem &&
                                                    med.SequencialCronograma == sequencialCronograma &&
                                                    med.ContratoRetificacaoItemId == contratoRetificacaoItemId &&
                                                    med.Id == id)
                                             select ((((med.Valor * itemImposto.PercentualBaseCalculo) / 100) * itemImposto.ImpostoFinanceiro.Aliquota) / 100)).Sum();

            return valorImpostoRetidoMedicao;
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

        public decimal ObterValorTotalMedidoLiberadoContrato(int? contratoId)
        {
            var valorTotalMedidoLiberadoContrato = (from med in ListaContratoRetificacaoItemMedicao
                                                    where med.ContratoId == contratoId
                                                    select med.Valor).Sum();

            return valorTotalMedidoLiberadoContrato;
        }

        public bool TemMedicaoALiberar(int? contratoId)
        {
            int quantidadeMedicaoALiberar = (from med in ListaContratoRetificacaoItemMedicao
                                             where ((med.ContratoId == contratoId) && 
                                                    (med.Situacao == SituacaoMedicao.AguardandoAprovacao || 
                                                     med.Situacao == SituacaoMedicao.AguardandoLiberacao))
                                             select med.Id).Count();
            bool temMedicaoALiberar = (quantidadeMedicaoALiberar > 0) ? true : false;

            return temMedicaoALiberar;
        }

    }
}
