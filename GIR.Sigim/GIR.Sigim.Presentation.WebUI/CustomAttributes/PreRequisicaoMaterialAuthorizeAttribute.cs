using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.Service.OrdemCompra;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Presentation.WebUI.CustomAttributes
{
    public class PreRequisicaoMaterialAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var parametrosOrdemCompraAppService = Container.Current.Resolve<IParametrosOrdemCompraAppService>();
            var parametrosOrdemCompra = parametrosOrdemCompraAppService.Obter();
            if (!parametrosOrdemCompra.EhPreRequisicaoMaterial)
                return false;

            return base.AuthorizeCore(httpContext);
        }
    }
}