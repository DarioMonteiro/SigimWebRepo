using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GIR.Sigim.Application.Resource;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.ViewModel.Admin;

namespace GIR.Sigim.Presentation.WebUI.Controllers
{
    public class AccountController : BaseController
    {
        private IUsuarioAppService usuarioService;

        public AccountController(IUsuarioAppService usuarioService, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.usuarioService = usuarioService;
        }
        
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (System.Web.HttpContext.Current.Request.IsAuthenticated)
                return RedirectToAction("Index", "Painel");

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            int timeout = Convert.ToInt32(ConfigurationManager.AppSettings["Timeout"]);
            if (ModelState.IsValid && usuarioService.Login(model.UserName, model.Password, model.RememberMe, timeout, Request.UserHostName))
                return RedirectToLocal(returnUrl);

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
                usuarioService.ChangePassword(model.OldPassword, model.NewPassword, model.ConfirmPassword);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout(FormCollection form)
        {
            usuarioService.Logout();

            return RedirectToAction("Index", "Painel");
        }

        public ActionResult Logout()
        {
            return RedirectToAction("Index", "Painel");
        }
    }
}