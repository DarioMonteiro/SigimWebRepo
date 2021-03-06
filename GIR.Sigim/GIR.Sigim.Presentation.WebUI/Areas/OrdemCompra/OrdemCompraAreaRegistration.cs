﻿using System.Web.Mvc;

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
                "OrdemCompra_unidadeMedida",
                "OrdemCompra/UnidadeMedida/{action}/{id}",
                new
                {
                    area = "OrdemCompra",
                    controller = "UnidadeMedida",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                new[] { "GIR.Sigim.Presentation.WebUI.Controllers" }
            );

            context.MapRoute(
                "OrdemCompra_material",
                "OrdemCompra/Material/{action}/{id}",
                new
                {
                    area = "OrdemCompra",
                    controller = "Material",
                    action = "Index",
                    id = UrlParameter.Optional
                },
                new string[] { "GIR.Sigim.Presentation.WebUI.Controllers" }
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