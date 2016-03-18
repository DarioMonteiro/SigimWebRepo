using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Contrato;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Application.Adapter;

namespace GIR.Sigim.Application.DTO.Financeiro
{
    public class TituloPagarDTO : AbstractTituloDTO
    {
        public ClienteFornecedorDTO Cliente { get; set; }
        public SituacaoTituloPagar Situacao { get; set; }
        public string SituacaoDescricao
        {
            get { return this.Situacao.ObterDescricao(); }
        }
        public TipoCompromissoDTO TipoCompromisso { get; set; }
        public TipoDocumentoDTO TipoDocumento { get; set; }


        public int? TituloPaiId { get; set; }
        public virtual TituloPagarDTO TituloPai { get; set; }
        public short? Parcela { get; set; }
        public decimal? ValorImposto { get; set; }
        public decimal? Desconto { get; set; }
        public Nullable<DateTime> DataLimiteDesconto { get; set; }
        public decimal? Multa { get; set; }
        public bool? EhMultaPercentual { get; set; }
        public decimal? TaxaPermanencia { get; set; }
        public bool? EhTaxaPermanenciaPercentual { get; set; }
        public string MotivoDesconto { get; set; }
        public Nullable<DateTime> DataEmissao { get; set; }
        public Nullable<DateTime> DataPagamento { get; set; }
        public Nullable<DateTime> DataBaixa { get; set; }
        public string LoginUsuarioCadastro { get; set; }
        public Nullable<DateTime> DataCadastro { get; set; }
        public string LoginUsuarioSituacao { get; set; }
        public Nullable<DateTime> DataSituacao { get; set; }
        public string LoginUsuarioApropriacao { get; set; }
        public Nullable<DateTime> DataApropriacao { get; set; }
        public FormaPagamento? FormaPagamento { get; set; }
        public string FormaPagamentoDescricao
        {
            get { return this.FormaPagamento.ObterDescricao(); }
        }
        public int? CodigoInterface { get; set; }
        public string SistemaOrigem { get; set; }
        public string CBBanco { get; set; }
        public string CBMoeda { get; set; }
        public string CBCampoLivre { get; set; }
        public string CBDV { get; set; }
        public string CBValor { get; set; }
        public string CBDataValor { get; set; }
        public bool? CBBarra { get; set; }
        public decimal? ValorPago { get; set; }
        public int? MotivoCancelamentoId { get; set; }
        public MotivoCancelamentoDTO MotivoCancelamento { get; set; }
        public int? MovimentoId { get; set; }
        public MovimentoFinanceiroDTO Movimento { get; set; }
        public int? BancoBaseBordero { get; set; }
        public string AgenciaContaBaseBordero { get; set; }
        public int? ContaCorrenteId { get; set; }
        public ContaCorrenteDTO ContaCorrente { get; set; }
        public decimal? Retencao { get; set; }
        public string Observacao { get; set; }
        public string MotivoCancelamentoInterface { get; set; }
        public bool? EhPagamentoAntecipado { get; set; }
        public string CBConcessionaria { get; set; }
        public int? TituloPrincipalAgrupamentoId { get; set; }
        public TituloPagarDTO TituloPrincipalAgrupamento { get; set; }

        public decimal ValorLiquido { get; set; }

        public List<TituloPagarDTO> ListaFilhos { get; set; }
        public List<ImpostoPagarDTO> ListaImpostoPagar { get; set; }
        public List<ApropriacaoDTO> ListaApropriacao { get; set; }

        public TituloPagarDTO()
        {
            this.ListaFilhos = new List<TituloPagarDTO>();
            this.ListaImpostoPagar = new List<ImpostoPagarDTO>();
            this.ListaApropriacao = new List<ApropriacaoDTO>();
        }
    }
}