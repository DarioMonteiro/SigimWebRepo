using System.Web.Mvc;

namespace GIR.Sigim.Presentation.WebUI.Areas.Sac
{
    public class SacAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Sac";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Sac_default",
                "Sac/{controller}/{action}/{id}",
                new
                {
                    area = "Sac",
                    controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );
        }
    }
}