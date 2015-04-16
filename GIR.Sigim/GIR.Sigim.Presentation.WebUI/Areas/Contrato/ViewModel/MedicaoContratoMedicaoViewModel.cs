using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;  
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Contrato;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;


namespace GIR.Sigim.Presentation.WebUI.Areas.Contrato.ViewModel
{
    public class MedicaoContratoMedicaoViewModel
    {
        public ContratoDTO Contrato { get; set; }
        
        public SelectList ListaServicoContratoRetificacaoItem { get; set; }
        public int ContratoRetificacaoItemId { get; set; }
        
        [Display(Name = "Retenção %")]
        public decimal? RetencaoContratual { get; set; }
        
        public decimal? ValorItem { get; set; }
        
        public ContratoRetificacaoItemDTO ContratoRetencaoItem { get; set; }
        
        [Display(Name="Tipo")]
        public SelectList ListaTipoDocumento { get; set; }
        public int? TipoDocumentoId { get; set; }
        
        [Display(Name = "Nº")]
        [StringLength(10, ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]       
        public string NumeroNotaFiscal { get; set; }

        [Display(Name = "Data emissão")]
        public Nullable<DateTime> DataEmissaoNotaFiscal { get; set; }

        [Display(Name = "Data vencimento")]
        public Nullable<DateTime> DataVencimentoNotaFiscal { get; set; }

        [Display(Name = "Multifornecedor")]
        public SelectList ListaMultiFornecedor { get; set; }
        public int? MultiFornecedorId { get; set; }

        [Display(Name = "Tipo compra")]
        public SelectList ListaTipoCompra { get; set; }
        public string TipoCompraCodigo { get; set; }

        [Display(Name = "CIF/FOB")]
        public SelectList ListaCifFob { get; set; }
        public int? CifFobId { get; set; }

        [Display(Name = "Natureza de operação")]
        public SelectList ListaNaturezaOperacao { get; set; }
        public string NaturezaOperacaoCodigo { get; set; }

        [Display(Name = "Série")]
        public SelectList ListaSerieNF { get; set; }
        public int? SerieNFId { get; set; }

        [Display(Name = "CST")]
        public SelectList ListaCST { get; set; }
        public string CSTCodigo { get; set; }

        [Display(Name = "Contribuição")]
        public SelectList ListaCodigoContribuicao { get; set; }
        public string CodigoContribuicaoCodigo { get; set; }

        [Display(Name = "Código de barras")]
        [StringLength(50, ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        public string CodigoBarras { get; set; }

    }
}