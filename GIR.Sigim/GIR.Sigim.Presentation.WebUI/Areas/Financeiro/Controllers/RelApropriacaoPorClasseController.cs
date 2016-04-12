using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel;
using Newtonsoft.Json;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Presentation.WebUI.CustomAttributes;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.Controllers
{
    public class RelApropriacaoPorClasseController : BaseController
    {
        private IApropriacaoAppService apropriacaoAppService;

        public RelApropriacaoPorClasseController(IApropriacaoAppService apropriacaoAppService,
                                                 MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.apropriacaoAppService = apropriacaoAppService;
        }

        [AutorizacaoAcessoAuthorize(GIR.Sigim.Application.Constantes.Modulo.FinanceiroWeb, Roles = Funcionalidade.RelatorioApropriacaoPorClasseAcessar)]
        public ActionResult Index()
        {
            var model = Session["Filtro"] as RelApropriacaoPorClasseListaViewModel;
            if (model == null)
            {
                model = new RelApropriacaoPorClasseListaViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
                model.Filtro.PaginationParameters.UniqueIdentifier = GenerateUniqueIdentifier();
                model.Filtro.DataInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                model.Filtro.DataFinal = DateTime.Now;
            }

            model.PodeImprimir = apropriacaoAppService.EhPermitidoImprimirRelApropriacaoPorClasse();

            CarregarListas(model);

            model.JsonItensClasseDespesa = JsonConvert.SerializeObject(new List<ClasseDTO>());
            model.JsonItensClasseReceita = JsonConvert.SerializeObject(new List<ClasseDTO>());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Imprimir(RelApropriacaoPorClasseListaViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;

                model.Filtro.ListaClasseDespesa = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ClasseDTO>>(model.JsonItensClasseDespesa);
                model.Filtro.ListaClasseReceita = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ClasseDTO>>(model.JsonItensClasseReceita);

                List<ApropriacaoClasseCCRelatorioDTO> listaApropriacaoPorClasseDTO = apropriacaoAppService.GerarRelatorioApropriacaoPorClasse(model.Filtro, Usuario.Id);
                if (listaApropriacaoPorClasseDTO == null)
                {
                    messageQueue.Add(Application.Resource.Sigim.ErrorMessages.InformacaoNaoEncontrada, TypeMessage.Error);
                    return PartialView("_NotificationMessagesPartial");
                }

                return Content("<script>executarImpressao();</script>");

            }
            return Content("<script>smartAlert(\"Atenção\", \"Ocorreu erro ao tentar imprimir !\", \"warning\")</script>");        }

        public ActionResult Imprimir(FormatoExportacaoArquivo formato)
        {
            var model = Session["Filtro"] as RelApropriacaoPorClasseListaViewModel;
            if (model == null)
            {
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NaoExistemRegistros, TypeMessage.Error);
                return PartialView("_NotificationMessagesPartial");
            }

            model.Filtro.ListaClasseDespesa = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ClasseDTO>>(model.JsonItensClasseDespesa);
            model.Filtro.ListaClasseReceita = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ClasseDTO>>(model.JsonItensClasseReceita);

            List<ApropriacaoClasseCCRelatorioDTO> listaApropriacaoPorClasseDTO = apropriacaoAppService.GerarRelatorioApropriacaoPorClasse(model.Filtro, Usuario.Id);
            if (listaApropriacaoPorClasseDTO == null)
            {
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.InformacaoNaoEncontrada, TypeMessage.Error);
                return PartialView("_NotificationMessagesPartial");
            }

            var arquivo = apropriacaoAppService.ExportarRelApropriacaoPorClasse(model.Filtro,listaApropriacaoPorClasseDTO, formato);

            if (arquivo != null)
            {
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                return File(arquivo.Stream, arquivo.ContentType, arquivo.NomeComExtensao);
            }

            return PartialView("_NotificationMessagesPartial");
        }

        private void CarregarListas(RelApropriacaoPorClasseListaViewModel model)
        {
            model.ListaOpcoesRelatorio = new SelectList(apropriacaoAppService.ListarOpcoesRelatorioApropriacaoPorClasse(), "Id", "Descricao", model.Filtro.OpcoesRelatorio);
        }

    }
}
