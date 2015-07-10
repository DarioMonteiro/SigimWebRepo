using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Infrastructure.Crosscutting.Security;
using GIR.Sigim.Presentation.WebUI.ViewModel;

namespace GIR.Sigim.Presentation.WebUI.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public MessageQueue messageQueue;

        public BaseController(MessageQueue messageQueue)
        {
            MergeMessages(messageQueue);
            System.Web.HttpContext.Current.Session["MessageQueue"] = messageQueue;
            this.messageQueue = messageQueue;
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

        public CustomPrincipal Usuario
        {
            get { return AuthenticationServiceFactory.Create().GetUser(); }
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
    }
}