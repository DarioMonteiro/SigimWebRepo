using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Presentation.WebUI.IoC
{
    public class ControllerFactory : DefaultControllerFactory
    {
        public ControllerFactory()
        {
            var controllerTypes =
                from t in Assembly.GetExecutingAssembly().GetTypes()
                where typeof(IController).IsAssignableFrom(t)
                select t;
            foreach (var t in controllerTypes)
            {
                Container.Current.RegisterType(t);
            }
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null) return null;
            return (IController)Container.Current.Resolve(controllerType);
        }
    }
}