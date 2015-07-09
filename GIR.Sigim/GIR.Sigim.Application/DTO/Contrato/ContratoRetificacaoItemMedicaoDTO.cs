using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.DTO.Contrato
{
    public class ContratoRetificacaoItemMedicaoDTO : BaseDTO
    {
        public int ContratoId { get; set; }
        //public ContratoDTO Contrato { get; set; }
        public int ContratoRetificacaoId { get; set; }
        public ContratoRetificacaoDTO ContratoRetificacao { get; set; }
        [Required]
        [Display(Name = "Item")]
        public int ContratoRetificacaoItemId { get; set; }
        public ContratoRetificacaoItemDTO ContratoRetificacaoItem { get; set; }
        public int SequencialItem { get; set; }
        public int ContratoRetificacaoItemCronogramaId { get; set; }
        public ContratoRetificacaoItemCronogramaDTO ContratoRetificacaoItemCronograma { get; set; }
        public int SequencialCronograma { get; set; }
        public SituacaoMedicao Situacao { get; set; }
        [Required]
        [Display(Name = "Tipo")]
        public int TipoDocumentoId { get; set; }
        public TipoDocumentoDTO TipoDocumento { get; set; }
        [Required]
        [Display(Name = "Nº")]
        [StringLength(10, ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        public string NumeroDocumento { get; set; }
        [Required]
        [Display(Name = "Data emissão")]
        public DateTime DataEmissao { get; set; }
        [Required]
        [Display(Name = "Data vencimento")]
        public DateTime DataVencimento { get; set; }
        [Required]
        [RegularExpression(@"^\d+(.\d+){0,1}$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Quantidade medição Atual")]
        public decimal Quantidade { get; set; }
        [Required]
        [RegularExpression(@"^\d+(.\d+){0,1}$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Valor medição atual")]
        public decimal Valor { get; set; }
        [Required]
        [Display(Name = "Data medição")]
        public DateTime DataMedicao { get; set; }
        public string UsuarioMedicao { get; set; }
        //public int? MultiFornecedorId { get; set; }
        public ClienteFornecedorDTO MultiFornecedor { get; set; }
        [StringLength(255, ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Observação")]
        public string Observacao { get; set; }
        public int? TituloPagarId { get; set; }
        public TituloPagarDTO TituloPagar { get; set; }
        public int? TituloReceberId { get; set; }
        public TituloReceberDTO TituloReceber { get; set; }
        public Nullable<DateTime> DataLiberacao { get; set; }
        public string UsuarioLiberacao { get; set; }
        public decimal? ValorRetido { get; set; }
        public string TipoCompraCodigo { get; set; }
        public TipoCompraDTO TipoCompra { get; set; }
        public int? CifFobId { get; set; }
        public CifFobDTO CifFob { get; set; }
        public string NaturezaOperacaoCodigo { get; set; }
        public NaturezaOperacaoDTO NaturezaOperacao { get; set; }
        public int? SerieNFId { get; set; }
        public SerieNFDTO SerieNF { get; set; }
        public string CSTCodigo { get; set; }
        public CSTDTO CST { get; set; }
        public string CodigoContribuicaoCodigo { get; set; }
        public CodigoContribuicaoDTO CodigoContribuicao { get; set; }
        [Display(Name = "Código de barras")]
        [StringLength(50, ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        public string CodigoBarras { get; set; }
        public decimal? BaseIPI { get; set; }
        public decimal? PercentualIpi { get; set; }
        public decimal? BaseIcms { get; set; }
        public decimal? PercentualIcms { get; set; }
        public decimal? BaseIcmsSt { get; set; }
        public decimal? PercentualIcmsSt { get; set; }
        public bool? Conferido { get; set; }
        public string UsuarioConferencia { get; set; }
        public Nullable<DateTime> DataConferencia { get; set; }
        public Nullable<DateTime> DataCadastro { get; set; }

        [RegularExpression(@"^\d+(.\d+){0,1}$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Desconto")]
        public decimal? Desconto { get; set; }

        [StringLength(30, ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Motivo desconto")]
        public string MotivoDesconto { get; set; }

        public decimal QuantidadeTotalMedida { get; set; }
        public decimal ValorTotalMedido { get; set; }
        public decimal QuantidadeTotalLiberada { get; set; }
        public decimal ValorTotalLiberado { get; set; }
        public decimal QuantidadeTotalMedidaLiberada { get; set; }
        public decimal ValorTotalMedidoLiberado { get; set; }
        public decimal QuantidadePendente
        {
            get { return Quantidade - QuantidadeTotalMedida; }
        }
        public decimal ValorPendente
        {
            get { return (Valor - ValorTotalMedido); }
        }
        public decimal ValorImpostoRetido { get; set; }
        public decimal ValorImpostoRetidoMedicao { get; set; }

        public decimal ValorImpostoIndiretoMedicao { get; set; }
        public decimal ValorTotalMedidoIndireto { get; set; }
        public decimal ValorTotalMedidoNota { get; set; }
        public decimal ValorTotalMedidoLiberadoContrato { get; set; }

        public ContratoRetificacaoItemMedicaoDTO()
        {
            //this.Contrato = new ContratoDTO();
            this.ContratoRetificacao = new ContratoRetificacaoDTO();
            this.ContratoRetificacaoItem = new ContratoRetificacaoItemDTO();
            this.ContratoRetificacaoItemCronograma = new ContratoRetificacaoItemCronogramaDTO();
            this.TipoDocumento = new TipoDocumentoDTO();
            this.MultiFornecedor = new ClienteFornecedorDTO();
            this.TituloPagar = new TituloPagarDTO();
            this.TituloReceber = new TituloReceberDTO();
            this.TipoCompra = new TipoCompraDTO();
            this.CifFob = new CifFobDTO();
            this.NaturezaOperacao = new NaturezaOperacaoDTO();
            this.SerieNF = new SerieNFDTO();
            this.CST = new CSTDTO();
            this.CodigoContribuicao = new CodigoContribuicaoDTO();

            this.DataEmissao = DateTime.Now;
            this.DataVencimento = DateTime.Now;
            this.DataMedicao = DateTime.Now;

            this.Desconto = 0;

            this.Situacao = 0;

        }
    }
}
