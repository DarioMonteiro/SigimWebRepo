using System.Web.Mvc;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro
{
    public class FinanceiroAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Financeiro";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               "FormaRecebimento",
               "Financeiro/FormaRecebimento/{action}/{id}",
               new
               {
                   area = "",
                   controller = "FormaRecebimento",
                   action = "Index",
                   id = UrlParameter.Optional
               },
               new[] { "GIR.Sigim.Presentation.WebUI.Controllers" }
           );

            context.MapRoute(
                "Financeiro_default",
                "Financeiro/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
