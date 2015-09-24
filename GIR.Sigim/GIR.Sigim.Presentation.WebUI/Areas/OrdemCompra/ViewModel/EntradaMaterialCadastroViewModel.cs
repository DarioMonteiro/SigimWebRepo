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
    public class EntradaMaterialCadastroViewModel
    {
        public EntradaMaterialDTO EntradaMaterial { get; set; }
        public SelectList ListaTipoNotaFiscal { get; set; }
        public SelectList ListaTipoCompra { get; set; }
        public SelectList ListaCifFob { get; set; }
        public SelectList ListaNaturezaOperacao { get; set; }
        public SelectList ListaSerieNF { get; set; }
        public SelectList ListaCST { get; set; }
        public SelectList ListaCodigoContribuicao { get; set; }
        public SelectList ListaComplementoNaturezaOperacao { get; set; }
        public SelectList ListaComplementoCST { get; set; }
        public SelectList ListaNaturezaReceita { get; set; }
        public SelectList ListaImpostoFinanceiro { get; set; }

        #region Accordion Itens

        public int? ItemId { get; set; }
        public int? Sequencial { get; set; }

        public MaterialDTO Material { get; set; }
        public ClasseDTO Classe { get; set; }
        public string JsonItens { get; set; }

        [Display(Name = "Complemento")]
        public string Complemento { get; set; }

        [RegularExpression(@"^\d+(.\d+){0,1}$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Quantidade")]
        public decimal Quantidade { get; set; }

        public decimal QuantidadeInicial { get; set; }
        public decimal Saldo { get; set; }

        [RegularExpression(@"^\d+(.\d+){0,1}$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Valor unitário")]
        public decimal ValorUnitario { get; set; }

        [RegularExpression(@"^\d+(.\d+){0,1}$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Desconto %")]
        public decimal PercentualDesconto { get; set; }

        [RegularExpression(@"^\d+(.\d+){0,1}$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Total")]
        public decimal ValorItem { get; set; }

        [RegularExpression(@"^\d+(.\d+){0,1}$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Base ICMS")]
        public decimal BaseICMS { get; set; }

        [RegularExpression(@"^\d+(.\d+){0,1}$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "ICMS %")]
        public decimal PercentualICMS { get; set; }

        [RegularExpression(@"^\d+(.\d+){0,1}$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Valor ICMS")]
        public decimal ValorICMS { get; set; }

        [RegularExpression(@"^\d+(.\d+){0,1}$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Base IPI")]
        public decimal BaseIPI { get; set; }

        [RegularExpression(@"^\d+(.\d+){0,1}$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "IPI %")]
        public decimal PercentualIPI { get; set; }

        [RegularExpression(@"^\d+(.\d+){0,1}$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Valor IPI")]
        public decimal ValorIPI { get; set; }

        [RegularExpression(@"^\d+(.\d+){0,1}$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Base ICMSST")]
        public decimal BaseICMSST { get; set; }

        [RegularExpression(@"^\d+(.\d+){0,1}$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "ICMSST %")]
        public decimal PercentualICMSST { get; set; }

        [RegularExpression(@"^\d+(.\d+){0,1}$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Valor ICMSST")]
        public decimal ValorICMSST { get; set; }

        public ComplementoNaturezaOperacaoDTO ComplementoNaturezaOperacao { get; set; }
        public ComplementoCSTDTO ComplementoCST { get; set; }
        public NaturezaReceitaDTO NaturezaReceita { get; set; }

        [Display(Name = "Total dos itens")]
        public decimal ValorTotalItem { get; set; }

        [Display(Name = "Descontos")]
        public decimal ValorTotalDesconto { get; set; }

        [Display(Name = "Desconto OC")]
        public decimal ValorTotalDescontoOrdemCompra { get; set; }

        [Display(Name = "Acréscimos")]
        public decimal ValorTotalAcrescimo { get; set; }

        [Display(Name = "Sub total")]
        public decimal ValorSubTotal { get; set; }

        [Display(Name = "Total")]
        public decimal ValorTotal { get; set; }

        #endregion

        #region Accordion Seleção de Formas de Pagamento

        [Display(Name = "OC")]
        public int? OrdemCompraId { get; set; }

        [Display(Name = "Data vencimento")]
        public Nullable<DateTime> DataVencimento { get; set; }

        [Display(Name = "Dias após emissão")]
        public int? QuantidadeDiasAposEmissao { get; set; }

        [Display(Name = "Data vencimento lib.")]
        public Nullable<DateTime> DataVencimentoLiberado { get; set; }

        [Display(Name = "Valor da parcela")]
        public decimal? ValorParcela { get; set; }

        [Display(Name = "Valor da parcela lib.")]
        public decimal? ValorParcelaLiberado { get; set; }

        [Display(Name = "Total EM")]
        public decimal? ValorTotalEM { get; set; }

        [Display(Name = "Total distribuído")]
        public decimal? ValorTotalDistribuido { get; set; }

        [Display(Name = "Total pendente")]
        public decimal? ValorTotalPendente { get; set; }

        [Display(Name = "Total liberado")]
        public decimal? ValorTotalLiberado { get; set; }

        #endregion

        #region Accordion Impostos
        public string JsonImpostos { get; set; }

        public ImpostoFinanceiroDTO ImpostoFinanceiro { get; set; }

        [Display(Name = "Base de cálculo")]
        public decimal BaseCalculoImposto { get; set; }

        [Display(Name = "Data vencimento")]
        public Nullable<DateTime> DataVencimentoImposto { get; set; }

        [Display(Name = "ICMS")]
        public decimal ValorTotalICMS { get; set; }

        [Display(Name = "IPI")]
        public decimal ValorTotalIPI{ get; set; }

        [Display(Name = "ICMSST")]
        public decimal ValorTotalICMSST { get; set; }

        [Display(Name = "Alíquota")]
        public decimal PercentualTotalAliquota { get; set; }
        
        [Display(Name = "Valor")]
        public decimal ValorTotalImposto { get; set; }

        [Display(Name = "Retido")]
        public decimal ValorTotalRetido { get; set; }

        #endregion

        public bool ExisteEstoqueParaCentroCusto { get; set; }
        public bool ExisteMovimentoNoEstoque { get; set; }
        public bool PodeSalvar { get; set; }
        public bool PodeCancelarEntrada { get; set; }
        public bool PodeImprimir { get; set; }
        public bool PodeLiberarTitulos { get; set; }
        public bool PodeAdicionarItem { get; set; }
        public bool PodeRemoverItem { get; set; }
        public bool PodeEditarItem { get; set; }
        public bool PodeAdicionarImposto { get; set; }
        public bool PodeRemoverImposto { get; set; }
        public bool PodeEditarImposto { get; set; }

        public bool PodeEditarCentroCusto { get; set; }
        public bool PodeEditarFornecedor { get; set; }

        public EntradaMaterialCadastroViewModel()
        {
            Material = new MaterialDTO();
            Classe = new ClasseDTO();
        }
    }
}