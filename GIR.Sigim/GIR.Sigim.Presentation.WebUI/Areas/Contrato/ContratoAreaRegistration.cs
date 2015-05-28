using System.Web.Mvc;

namespace GIR.Sigim.Presentation.WebUI.Areas.Contrato
{
    public class ContratoAreaRegistration : AreaRegistration 
    {

        public override string AreaName
        {
            get 
            {
                return "Contrato";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Contrato_default",
                "Contrato/{controller}/{action}/{id}",
                new { action ="Index", id = UrlParameter.Optional });
        }

    }
}
