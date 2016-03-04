using System.Web.Mvc;

namespace GIR.Sigim.Presentation.WebUI.Areas.Comercial
{
    public class ComercialAreaRegistration : AreaRegistration 
    {

        public override string AreaName
        {
            get 
            {
                return "Comercial";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Comercial_default",
                "Comercial/{controller}/{action}/{id}",
                new { action ="Index", id = UrlParameter.Optional });
        }

    }
}
