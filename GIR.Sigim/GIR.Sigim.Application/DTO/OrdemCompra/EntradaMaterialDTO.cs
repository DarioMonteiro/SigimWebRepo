using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Estoque;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Application.DTO.OrdemCompra
{
    public class EntradaMaterialDTO : BaseDTO
    {
        public CentroCustoDTO CentroCusto { get; set; }
        public string CentroCustoDescricao
        {
            get { return this.CentroCusto.Codigo + " - " + this.CentroCusto.Descricao; }
        }

        private ClienteFornecedorDTO clienteFornecedor;
        public ClienteFornecedorDTO ClienteFornecedor
        {
            get { return clienteFornecedor ?? new ClienteFornecedorDTO(); }
            set { clienteFornecedor = value; }
        }

        private ClienteFornecedorDTO fornecedorNota;
        public ClienteFornecedorDTO FornecedorNota
        {
            get { return fornecedorNota ?? new ClienteFornecedorDTO(); }
            set { fornecedorNota = value; }
        }

        public string FornecedorNome
        {
            get
            {
                string nome = string.Empty;
                if (FornecedorNota != null)
                    nome = FornecedorNota.Nome;
                else
                    nome = ClienteFornecedor.Nome;

                return nome;
            }
        }
        public DateTime Data { get; set; }
        public SituacaoEntradaMaterial Situacao { get; set; }
        [Display(Name = "Situação")]
        public string SituacaoDescricao
        {
            get { return this.Situacao.ObterDescricao(); }
        }

        [Display(Name = "Tipo")]
        public int? TipoNotaFiscalId { get; set; }
        public TipoDocumentoDTO TipoNotaFiscal { get; set; }

        [Display(Name = "Número")]
        public string NumeroNotaFiscal { get; set; }

        [Display(Name = "Data da emissão")]
        public Nullable<DateTime> DataEmissaoNota { get; set; }

        [Display(Name = "Data da entrega")]
        public Nullable<DateTime> DataEntregaNota { get; set; }

        public string Observacao { get; set; }

        [Display(Name = "Percentual")]
        public bool EhDescontoPercentual { get; set; }

        [Display(Name = "Descontos")]
        public decimal? Desconto { get; set; }

        public decimal? PercentualISS { get; set; }
        public decimal? ISS { get; set; }
        public decimal? FreteIncluso { get; set; }

        [Display(Name = "Data cadastro")]
        public Nullable<DateTime> DataCadastro { get; set; }

        [Display(Name = "Cadastrado por")]
        public string LoginUsuarioCadastro { get; set; }

        [Display(Name = "Data liberação")]
        public Nullable<DateTime> DataLiberacao { get; set; }

        [Display(Name = "Liberado por")]
        public string LoginUsuarioLiberacao { get; set; }

        public Nullable<DateTime> DataCancelamento { get; set; }
        public string LoginUsuarioCancelamento { get; set; }
        public string MotivoCancelamento { get; set; }
        public int? TransportadoraId { get; set; }
        public Nullable<DateTime> DataFrete { get; set; }
        public decimal? ValorFrete { get; set; }
        public int? TipoNotaFreteId { get; set; }
        //public TipoDocumento TipoNotaFrete { get; set; }
        public string NumeroNotaFrete { get; set; }
        public int? OrdemCompraFrete { get; set; }
        public int? TituloFrete { get; set; }

        [Display(Name = "Tipo de compra")]
        public string CodigoTipoCompra { get; set; }
        public TipoCompraDTO TipoCompra { get; set; }

        [Display(Name = "CIF/FOB")]
        public int? CifFobId { get; set; }
        public CifFobDTO CifFob { get; set; }

        [Display(Name = "Natureza da operação")]
        public string CodigoNaturezaOperacao { get; set; }
        public NaturezaOperacaoDTO NaturezaOperacao { get; set; }

        [Display(Name = "Série")]
        public int? SerieNFId { get; set; }
        public SerieNFDTO SerieNF { get; set; }

        [Display(Name = "CST")]
        public string CodigoCST { get; set; }
        public CSTDTO CST { get; set; }

        [Display(Name = "Contribuição")]
        public string CodigoContribuicaoId { get; set; }
        public CodigoContribuicaoDTO CodigoContribuicao { get; set; }

        [Display(Name = "Código de barras")]
        public string CodigoBarras { get; set; }
        public bool? EhConferido { get; set; }
        public Nullable<DateTime> DataConferencia { get; set; }
        public string LoginUsuarioConferencia { get; set; }
        public bool PossuiAvaliacaoFornecedor { get; set; }
        public List<EntradaMaterialItemDTO> ListaItens { get; set; }
        public List<EntradaMaterialFormaPagamentoDTO> ListaFormaPagamento { get; set; }
        public List<EntradaMaterialImpostoDTO> ListaImposto { get; set; }
        public List<MovimentoDTO> ListaMovimentoEstoque { get; set; }

        public EntradaMaterialDTO()
        {
            this.Situacao = SituacaoEntradaMaterial.Pendente;
            this.Data = DateTime.Now;
            this.CentroCusto = new CentroCustoDTO();
            this.ClienteFornecedor = new ClienteFornecedorDTO();
            this.FornecedorNota = new ClienteFornecedorDTO();
            this.ListaItens = new List<EntradaMaterialItemDTO>();
            this.ListaFormaPagamento = new List<EntradaMaterialFormaPagamentoDTO>();
            this.ListaImposto = new List<EntradaMaterialImpostoDTO>();
            this.ListaMovimentoEstoque = new List<MovimentoDTO>();
        }
    }
}