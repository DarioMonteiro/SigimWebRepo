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
        public SelectList ListaServicoContratoRetificacaoItem { get; set; }
        public ContratoRetificacaoItemMedicaoDTO ContratoRetificacaoItemMedicao { get; set; }
        public ContratoDTO Contrato { get; set; }
        public ContratoRetificacaoDTO ContratoRetificacao { get; set; }
        public ResumoLiberacaoDTO Resumo { get; set; }
        public string JsonListaItemLiberacao { get; set; }

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

        public LiberacaoContratoLiberacaoViewModel()
        {
            ContratoRetificacaoItemMedicao = new ContratoRetificacaoItemMedicaoDTO();
            Contrato = new ContratoDTO();
            Resumo = new ResumoLiberacaoDTO();
        }
    }
}