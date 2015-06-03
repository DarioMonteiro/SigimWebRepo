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

        public SelectList ListaServicoContratoRetificacaoItem { get; set; }

        [Display(Name = "Retenção %")]
        public decimal? RetencaoContratual { get; set; }             

        public ContratoRetificacaoItemMedicaoDTO ContratoRetificacaoItemMedicao { get; set; }
        
        [Display(Name="Tipo")]
        public SelectList ListaTipoDocumento { get; set; }

        [Display(Name = "Multifornecedor")]
        public SelectList ListaMultiFornecedor { get; set; }

        [Display(Name = "Tipo compra")]
        public SelectList ListaTipoCompra { get; set; }

        [Display(Name = "CIF/FOB")]
        public SelectList ListaCifFob { get; set; }

        [Display(Name = "Natureza de operação")]
        public SelectList ListaNaturezaOperacao { get; set; }

        [Display(Name = "Série")]
        public SelectList ListaSerieNF { get; set; }

        [Display(Name = "CST")]
        public SelectList ListaCST { get; set; }

        [Display(Name = "Contribuição")]
        public SelectList ListaCodigoContribuicao { get; set; }

        public string JsonListaRetificacaoProvisao { get; set; }
        public string JsonListaContratoRetificacaoItemMedicao { get; set; }
        public int? DiasPagamentoParametrosContrato { get; set; }
        public int? DiasMedicaoParametrosContrato { get; set; }
        public Nullable<DateTime> DataLimiteMedicao { get; set; }

        public bool EhSituacaoAguardandoAprovacao { get; set; }
        public bool EhSituacaoAguardandoLiberacao { get; set; }
        public bool EhSituacaoLiberado { get; set; }

        public bool PodeSalvar { get; set; }
        public bool PodeCancelar { get; set; }

        [Display(Name = "Preço unitário")]
        public decimal PrecoUnitario { get; set; }

        public MedicaoContratoMedicaoViewModel()
        {
            this.ContratoRetificacaoItemMedicao = new ContratoRetificacaoItemMedicaoDTO();

            EhSituacaoAguardandoAprovacao = true;
            EhSituacaoAguardandoLiberacao = false;
            EhSituacaoLiberado = false;

        }

    }
}