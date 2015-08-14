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
        public bool ExisteEstoqueParaCentroCusto { get; set; }
        public bool ExisteMovimentoNoEstoque { get; set; }
        public bool PodeSalvar { get; set; }
        public bool PodeCancelarEntrada { get; set; }
        public bool PodeImprimir { get; set; }
    }
}