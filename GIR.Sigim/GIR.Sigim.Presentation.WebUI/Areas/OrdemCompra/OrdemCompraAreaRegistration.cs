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