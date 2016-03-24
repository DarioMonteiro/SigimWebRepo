using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Estoque;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public class EntradaMaterial : BaseEntity
    {
        public string CodigoCentroCusto { get; set; }
        public CentroCusto CentroCusto { get; set; }
        public int? ClienteFornecedorId { get; set; }
        public ClienteFornecedor ClienteFornecedor { get; set; }
        public int? FornecedorNotaId { get; set; }
        public ClienteFornecedor FornecedorNota { get; set; }
        public DateTime Data { get; set; }
        public SituacaoEntradaMaterial Situacao { get; set; }
        public int? TipoNotaFiscalId { get; set; }
        public TipoDocumento TipoNotaFiscal { get; set; }
        public string NumeroNotaFiscal { get; set; }
        public Nullable<DateTime> DataEmissaoNota { get; set; }
        public Nullable<DateTime> DataEntregaNota { get; set; }
        public string Observacao { get; set; }
        public decimal? PercentualDesconto { get; set; }

        private decimal? desconto;
        public decimal? Desconto
        {
            get { return desconto.HasValue ? desconto : 0; }
            set { desconto = value; }
        }

        public decimal? PercentualISS { get; set; }
        public decimal? ISS { get; set; }
        
        private decimal? freteIncluso;
        public decimal? FreteIncluso
        {
            get { return freteIncluso.HasValue ? freteIncluso : 0; }
            set { freteIncluso = value; }
        }
        public DateTime DataCadastro { get; set; }
        public string LoginUsuarioCadastro { get; set; }
        public Nullable<DateTime> DataLiberacao { get; set; }
        public string LoginUsuarioLiberacao { get; set; }
        public Nullable<DateTime> DataCancelamento { get; set; }
        public string LoginUsuarioCancelamento { get; set; }
        public string MotivoCancelamento { get; set; }
        public int? TransportadoraId { get; set; }
        public ClienteFornecedor Transportadora { get; set; }
        public Nullable<DateTime> DataFrete { get; set; }
        public decimal? ValorFrete { get; set; }
        public int? TipoNotaFreteId { get; set; }
        public TipoDocumento TipoNotaFrete { get; set; }
        public string NumeroNotaFrete { get; set; }
        public int? OrdemCompraFreteId { get; set; }
        public OrdemCompra OrdemCompraFrete { get; set; }
        public int? TituloFreteId { get; set; }
        public TituloPagar TituloFrete { get; set; }
        public string CodigoTipoCompra { get; set; }
        public TipoCompra TipoCompra { get; set; }
        public int? CifFobId { get; set; }
        public CifFob CifFob { get; set; }
        public string CodigoNaturezaOperacao { get; set; }
        public NaturezaOperacao NaturezaOperacao { get; set; }
        public int? SerieNFId { get; set; }
        public SerieNF SerieNF { get; set; }
        public string CodigoCST { get; set; }
        public CST CST { get; set; }
        public string CodigoContribuicaoId { get; set; }
        public CodigoContribuicao CodigoContribuicao { get; set; }
        public string CodigoBarras { get; set; }
        public bool? EhConferido { get; set; }
        public Nullable<DateTime> DataConferencia { get; set; }
        public string LoginUsuarioConferencia { get; set; }

        public decimal? ValorTotalFormasPagamento
        {
            get { return this.ListaFormaPagamento.Sum(l => l.Valor); }
        }

        public decimal? ValorTotalItens {
            get { return this.ListaItens.Sum(l => l.ValorTotal + l.ValorIPI); }
        }

        public decimal? ValorTotalDescontoOrdemCompra
        {
            get { return this.ListaItens.Sum(l => l.ValorDescontoOrdemCompra); }
        }

        public decimal? ValorSubTotal
        {
            get { return this.ValorTotalItens - this.ValorTotalDescontoOrdemCompra; }
        }

        public decimal? ValorDesconto
        {
            get { return this.PercentualDesconto.HasValue ? this.ValorSubTotal * this.Desconto / 100 : this.Desconto; }
        }

        public decimal? ValorTotalICMS
        {
            get { return this.ListaItens.Sum(l => l.ValorICMS); }
        }

        public decimal? ValorTotalRetido
        {
            get { return this.ListaImposto.Where(l => l.ImpostoFinanceiro.EhRetido.Value).Sum(o => o.Valor); }
        }

        public decimal? ValorDedutivel
        {
            get { return this.ValorTotalICMS + this.ValorTotalRetido; }
        }

        public decimal? ValorTotalDesconto
        {
            get { return this.ValorDesconto + this.ValorDedutivel; }
        }

        public decimal? ValorTotal
        {
            get { return this.ValorTotalItens - this.ValorTotalDesconto + this.FreteIncluso; }
        }

        public ICollection<EntradaMaterialItem> ListaItens { get; set; }
        public ICollection<EntradaMaterialFormaPagamento> ListaFormaPagamento { get; set; }
        public ICollection<EntradaMaterialImposto> ListaImposto { get; set; }
        public ICollection<AvaliacaoFornecedor> ListaAvaliacaoFornecedor { get; set; }
        public ICollection<Movimento> ListaMovimentoEstoque { get; set; }
        public ICollection<OrdemCompra> ListaOrdemCompraFrete { get; set; }
       
        public EntradaMaterial()
        {
            this.ListaItens = new HashSet<EntradaMaterialItem>();
            this.ListaFormaPagamento = new HashSet<EntradaMaterialFormaPagamento>();
            this.ListaImposto = new HashSet<EntradaMaterialImposto>();
            this.ListaAvaliacaoFornecedor = new HashSet<AvaliacaoFornecedor>();
            this.ListaMovimentoEstoque = new HashSet<Movimento>();
            this.ListaOrdemCompraFrete = new HashSet<OrdemCompra>();
        }
    }
}