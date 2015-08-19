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
             "ContaCorrente",
             "Financeiro/ContaCorrente/{action}/{id}",
             new
             {
                 area = "Financeiro",
                 controller = "ContaCorrente",
                 action = "Index",
                 id = UrlParameter.Optional
             },
             new[] { "GIR.Sigim.Presentation.WebUI.Controllers" }
         );

            context.MapRoute(
             "Agencia",
             "Financeiro/Agencia/{action}/{id}",
             new
             {
                 area = "Financeiro",
                 controller = "Agencia",
                 action = "Index",
                 id = UrlParameter.Optional
             },
             new[] { "GIR.Sigim.Presentation.WebUI.Controllers" }
         );

            context.MapRoute(
              "Banco",
              "Financeiro/Banco/{action}/{id}",
              new
              {
                  area = "Financeiro",
                  controller = "Banco",
                  action = "Index",
                  id = UrlParameter.Optional
              },
              new[] { "GIR.Sigim.Presentation.WebUI.Controllers" }
          );

            context.MapRoute(
               "FormaRecebimento",
               "Financeiro/FormaRecebimento/{action}/{id}",
               new
               {
                   area = "Financeiro",
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
