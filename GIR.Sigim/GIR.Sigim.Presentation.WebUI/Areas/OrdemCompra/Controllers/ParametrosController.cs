using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Application.Service.OrdemCompra;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Areas.OrdemCompra.ViewModel;
using GIR.Sigim.Presentation.WebUI.Controllers;

namespace GIR.Sigim.Presentation.WebUI.Areas.OrdemCompra.Controllers
{
    public class ParametrosController : BaseController
    {
        private IParametrosOrdemCompraAppService parametrosAppService;
        private IAssuntoContatoAppService assuntoContatoAppService;
        private IClienteFornecedorAppService clienteFornecedorAppService;
        private ITipoCompromissoAppService tipoCompromissoAppService;
        private IBancoLayoutAppService bancoLayoutAppService;

        public ParametrosController(
            IParametrosOrdemCompraAppService parametrosAppService,
            IAssuntoContatoAppService assuntoContatoAppService,
            IClienteFornecedorAppService clienteFornecedorAppService,
            ITipoCompromissoAppService tipoCompromissoAppService,
            IBancoLayoutAppService bancoLayoutAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.parametrosAppService = parametrosAppService;
            this.assuntoContatoAppService = assuntoContatoAppService;
            this.clienteFornecedorAppService = clienteFornecedorAppService;
            this.tipoCompromissoAppService = tipoCompromissoAppService;
            this.bancoLayoutAppService = bancoLayoutAppService;
        }

        [Authorize(Roles = Funcionalidade.ParametroOrdemCompraAcessar)]
        public ActionResult Index()
        {
            ParametrosViewModel model = new ParametrosViewModel();
            model.Parametros = parametrosAppService.Obter() ?? new ParametrosOrdemCompraDTO();
            model.PodeSalvar = parametrosAppService.EhPermitidoSalvar();
            CarregarCombos(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ParametrosViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.IconeRelatorio != null)
                {
                    using (Stream inputStream = model.IconeRelatorio.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        model.Parametros.IconeRelatorio = memoryStream.ToArray();
                    }
                }
                if (parametrosAppService.Salvar(model.Parametros))
                    return PartialView("Redirect", Url.Action("Index", "Parametros", new { area = "OrdemCompra" }));
            }

            return PartialView("_NotificationMessagesPartial");
        }

        private void CarregarCombos(ParametrosViewModel model)
        {
            int? assuntoContatoId = null;
            int? tipoCompromissoFreteId = null;
            int? layoutSPEDId = null;
            int? interfaceCotacaoModelo = null;

            if (model.Parametros != null)
            {
                assuntoContatoId = model.Parametros.AssuntoContatoId;
                tipoCompromissoFreteId = model.Parametros.TipoCompromissoFreteId;
                layoutSPEDId = model.Parametros.LayoutSPEDId;
                interfaceCotacaoModelo = model.Parametros.InterfaceCotacao.Modelo;
            }
            
            model.ListaAssuntoContatoEmail = new SelectList(assuntoContatoAppService.ListarTodos(), "Id", "Descricao", assuntoContatoId);
            model.ListaTipoCompromissoFrete = new SelectList(tipoCompromissoAppService.ListarTipoPagar(), "Id", "Descricao", tipoCompromissoFreteId);
            model.ListaLayoutSPED = new SelectList(bancoLayoutAppService.ListarPeloTipoInterfaceSpedFiscal(), "Id", "Descricao", layoutSPEDId);
            model.ListaModeloInterface = new SelectList(bancoLayoutAppService.ListarPeloTipoInterfaceCotacao(), "Id", "Descricao", interfaceCotacaoModelo);
        }
    }
}