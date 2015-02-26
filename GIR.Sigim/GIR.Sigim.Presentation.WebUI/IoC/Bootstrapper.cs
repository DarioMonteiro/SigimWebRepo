using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GIR.Sigim.Presentation.WebUI.IoC
{
    public class Bootstrapper
    {
        public static void Initialise()
        {
            ControllerBuilder.Current.SetControllerFactory(new ControllerFactory());
            GIR.Sigim.Application.Helper.MapperHelper.Initialise();
        }
    }
}