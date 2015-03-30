using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Controllers;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(MessageQueue messageQueue)
            : base(messageQueue)
        {

        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
