using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;  
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Contrato;

namespace GIR.Sigim.Presentation.WebUI.Areas.Contrato.ViewModel
{
    public class LiberacaoContratoLiberacaoViewModel
    {
        public int ContratoRetificacaoItemIdSelecionado { get; set; }
        public int ContratoRetificacaoItemMedicaoIdSelecionado { get; set; }
        public SelectList ListaServicoContratoRetificacaoItem { get; set; }
        public ContratoRetificacaoItemMedicaoDTO ContratoRetificacaoItemMedicao { get; set; }
        public ContratoDTO Contrato { get; set; }
        public ContratoRetificacaoDTO ContratoRetificacao { get; set; }
        public ResumoLiberacaoDTO Resumo { get; set; }
        public string JsonListaItemLiberacao { get; set; }

        [Required]
        [Display(Name = "Data Vencimento")]
        public Nullable<DateTime> DataVencimento { get; set; }

        public bool PodeHabilitarBotoes { get; set; }
        public bool PodeAprovarLiberar { get; set; }
        public bool PodeAprovar { get; set; }
        public bool PodeLiberar { get; set; }
        public bool PodeCancelarLiberacao { get; set; }
        public bool PodeAssociarNF { get; set; }
        public bool PodeAssociarNotaFiscal { get; set; }
        public bool PodeAlterarDataVencimento { get; set; }
        public bool PodeImprimirMedicao { get; set; }
        public bool PodeConcluirContrato { get; set; }

        public string TipoDocumentoAntigo { get; set; }
        public string NumeroDocumentoAntigo { get; set; }
        public string DataEmissaoAntigo { get; set; }
        public string DataVencimentoAntigo { get; set; }

        [Required]
        public int TipoDocumentoNovoId { get; set; }
        [Required]
        [StringLength(10, ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        public string NumeroDocumentoNovo { get; set; }
        [Required]
        public DateTime DataEmissaoNovo { get; set; }
        [Required]
        public DateTime DataVencimentoNovo { get; set; }

        public SelectList ListaTipoDocumentoNovo { get; set; }

        public LiberacaoContratoLiberacaoViewModel()
        {
            ContratoRetificacaoItemMedicao = new ContratoRetificacaoItemMedicaoDTO();
            Contrato = new ContratoDTO();
            Resumo = new ResumoLiberacaoDTO();

            this.DataEmissaoNovo = DateTime.Now;
            this.DataVencimentoNovo = DateTime.Now;

        }
    }
}