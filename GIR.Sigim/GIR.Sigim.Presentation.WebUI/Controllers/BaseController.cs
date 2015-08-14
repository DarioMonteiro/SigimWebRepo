using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Application.Service.OrdemCompra;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Infrastructure.Crosscutting.Security;
using GIR.Sigim.Presentation.WebUI.ViewModel;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Presentation.WebUI.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public MessageQueue messageQueue;

        private CustomPrincipal usuario;
        public CustomPrincipal Usuario
        {
            get
            {
                if (usuario == null)
                    usuario = AuthenticationServiceFactory.Create().GetUser();

                return usuario;
            }
        }

        public BaseController(MessageQueue messageQueue) : base()
        {
            MergeMessages(messageQueue);
            System.Web.HttpContext.Current.Session["MessageQueue"] = messageQueue;
            this.messageQueue = messageQueue;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            ObterPermissoesUsuario();
            RemoverPermissoesUsuarioConformeParametros();
        }

        private void ObterPermissoesUsuario()
        {
            var usuarioAppService = Container.Current.Resolve<IUsuarioAppService>();
            if (Usuario != null)
                Usuario.Roles = usuarioAppService.ObterPermissoesUsuario(Usuario.Id);
        }

        private void RemoverPermissoesUsuarioConformeParametros()
        {
            if (!this.HttpContext.Request.IsAjaxRequest())
            {
                string area = this.ControllerContext.RouteData.DataTokens["area"] as string;
                switch (area)
                {
                    case "OrdemCompra":
                        var parametrosOrdemCompraAppService = Container.Current.Resolve<IParametrosOrdemCompraAppService>();
                        var parametrosOrdemCompra = parametrosOrdemCompraAppService.Obter();
                        if (!parametrosOrdemCompra.EhPreRequisicaoMaterial)
                            Usuario.Roles = Usuario.Roles.Where(l => l != Funcionalidade.PreRequisicaoMaterialAcessar).ToArray();
                        break;
                }
            }
        }

        private static void MergeMessages(MessageQueue messageQueue)
        {
            var previousMessages = System.Web.HttpContext.Current.Session["MessageQueue"] as MessageQueue;

            if (previousMessages != null)
            {
                foreach (var message in previousMessages.GetAll())
                {
                    messageQueue.Add(message.Text, message.Type);
                }
            }
        }

        protected void AdicionarMensagemNotificacao(string mensagem, TypeMessage tipo)
        {
            messageQueue.Add(mensagem, tipo);
        }

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Painel");
            }
        }

        public string[] PageSizeList
        {
            get
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["PageSize"].Split('|');
            }
        }

        protected int DefaultPageSize
        {
            get
            {
                return Convert.ToInt32(PageSizeList[0]);
            }
        }

        protected int PaginationListSize
        {
            get
            {
                return Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["PaginationListSize"]);
            }
        }

        protected ListaViewModel CreateListaViewModel(PaginationParameters paginationParameters, int totalRecords, object records)
        {
            var listaViewModel = new ListaViewModel();
            listaViewModel.Records = records;
            listaViewModel.PageIndex = paginationParameters.PageIndex;
            listaViewModel.PageSize = paginationParameters.PageSize;
            listaViewModel.Ascending = paginationParameters.Ascending;
            listaViewModel.OrderBy = paginationParameters.OrderBy;
            listaViewModel.UniqueIdentifier = paginationParameters.UniqueIdentifier;
            listaViewModel.PageSizeList = new SelectList(this.PageSizeList, paginationParameters.PageSize);
            listaViewModel.Pagination = new Pagination(
                listaViewModel.PageIndex,
                listaViewModel.PageSize,
                totalRecords,
                this.PaginationListSize);

            return listaViewModel;
        }

        protected ListaViewModel CreateListaViewModel(object records)
        {
            var listaViewModel = new ListaViewModel();
            listaViewModel.Records = records;
            return listaViewModel;
        }

        protected string GenerateUniqueIdentifier()
        {
            return "_" + Guid.NewGuid().ToString().Replace("-", string.Empty);
        }
    }
}