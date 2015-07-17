using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
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
        public ClienteFornecedorDTO ClienteFornecedor { get; set; }
        public ClienteFornecedorDTO FornecedorNota { get; set; }
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

        //TipoNotaFiscal
        public string NumeroNotaFiscal { get; set; }
        public Nullable<DateTime> DataEmissaoNota { get; set; }
        public Nullable<DateTime> DataEntregaNota { get; set; }
        public string Observacao { get; set; }
        public decimal? PercentualDesconto { get; set; }
        public decimal? Desconto { get; set; }
        public decimal? PercentualISS { get; set; }
        public decimal? ISS { get; set; }
        public decimal? FreteIncluso { get; set; }
        public Nullable<DateTime> DataCadastro { get; set; }
        public string LoginUsuarioCadastro { get; set; }
        public Nullable<DateTime> DataLiberacao { get; set; }
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
        //TipoCompra
        //CIFFOB
        //NaturezaOperacao
        //SerieNF
        //CST
        //CodigoContribuicao
        public string CodigoBarras { get; set; }
        public bool? EhConferido { get; set; }
        public Nullable<DateTime> DataConferencia { get; set; }
        public string LoginUsuarioConferencia { get; set; }
        public bool PossuiAvaliacaoFornecedor { get; set; }
        public ICollection<EntradaMaterialItemDTO> ListaItens { get; set; }
        public ICollection<EntradaMaterialFormaPagamentoDTO> ListaFormaPagamento { get; set; }
        public ICollection<EntradaMaterialImpostoDTO> ListaImposto { get; set; }

        public EntradaMaterialDTO()
        {
            this.Situacao = SituacaoEntradaMaterial.Pendente;
            this.Data = DateTime.Now;
            this.CentroCusto = new CentroCustoDTO();
            this.ClienteFornecedor = new ClienteFornecedorDTO();
            this.FornecedorNota = new ClienteFornecedorDTO();
            this.ListaItens = new HashSet<EntradaMaterialItemDTO>();
            this.ListaFormaPagamento = new HashSet<EntradaMaterialFormaPagamentoDTO>();
            this.ListaImposto = new HashSet<EntradaMaterialImpostoDTO>();
        }
    }
}