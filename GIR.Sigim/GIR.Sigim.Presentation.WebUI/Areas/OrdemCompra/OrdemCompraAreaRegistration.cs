using System.Web.Mvc;

namespace GIR.Sigim.Presentation.WebUI.Areas.OrdemCompra
{
    public class OrdemCompraAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "OrdemCompra";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Material",
                "OrdemCompra/Material/{action}/{id}",
                new
                {
                    area = "",
                    controller = "Material",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                new [] { "GIR.Sigim.Presentation.WebUI.Controllers" }
            );

            context.MapRoute(
                "OrdemCompra_default",
                "OrdemCompra/{controller}/{action}/{id}",
                new
                {
                    area = "OrdemCompra",
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );
        }
    }
}