using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Orcamento;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.OrdemCompras;
using GIR.Sigim.Presentation.WebUI.ViewModel;

namespace GIR.Sigim.Presentation.WebUI.Areas.OrdemCompra.ViewModel
{
    public class RequisicaoMaterialCadastroViewModel
    {
        public RequisicaoMaterialDTO RequisicaoMaterial { get; set; }

        public int? ItemId { get; set; }

        public ClasseDTO Classe { get; set; }

        public MaterialDTO Material { get; set; }

        //public int? InsumoId { get; set; }

        //public int? ComposicaoId { get; set; }

        public OrcamentoInsumoRequisitadoDTO OrcamentoInsumoRequisitado { get; set; }

        [Display(Name = "Sequencial")]
        public string Sequencial { get; set; }

        [Display(Name = "Complemento")]
        public string Complemento { get; set; }

        [RegularExpression(@"^\d+(.\d+){0,1}$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Quantidade")]
        public decimal Quantidade { get; set; }

        [RegularExpression(@"^\d+(.\d+){0,1}$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Qtd. aprovada")]
        public decimal QuantidadeAprovada { get; set; }

        [Display(Name = "Data mínima")]
        public Nullable<DateTime> DataMinima { get; set; }

        [Display(Name = "Data máxima")]
        public Nullable<DateTime> DataMaxima { get; set; }

        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Prazo")]
        public int? Prazo { get; set; }

        public string JsonItens { get; set; }

        public bool PodeSalvar { get; set; }
        public bool PodeCancelarRequisicao { get; set; }
        public bool PodeImprimir { get; set; }
        public bool PodeAdicionarItem { get; set; }
        public bool PodeCancelarItem { get; set; }
        public bool PodeEditarItem { get; set; }
        public bool PodeAprovarRequisicao { get; set; }
        public bool PodeCancelarAprovacao { get; set; }
        public bool PodeEditarCentroCusto { get; set; }

        public RequisicaoMaterialCadastroViewModel()
        {
            RequisicaoMaterial = new RequisicaoMaterialDTO();
            Material = new MaterialDTO();
        }
    }
}