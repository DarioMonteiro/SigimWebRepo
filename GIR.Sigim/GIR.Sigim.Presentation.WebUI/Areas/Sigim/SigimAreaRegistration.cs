using System.Web.Mvc;

namespace GIR.Sigim.Presentation.WebUI.Areas.Sigim
{
    public class SigimAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Sigim";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Sigim_default",
                "Sigim/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
