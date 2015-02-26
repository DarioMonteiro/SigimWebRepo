using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GIR.Sigim.Presentation.WebUI.Helpers
{
    public static class CustomValidationSummaryHelper
    {
        public static string CustomValidationSummary(this HtmlHelper htmlHelper, bool excludePropertyErrors)
        {
            StringBuilder stringRetorno = new StringBuilder();
            if (!htmlHelper.ViewData.ModelState.IsValid)
            {
                Func<string, bool> expression = (l => true);
                if (excludePropertyErrors)
                    expression = (l => l.Length == 0);

                foreach (var key in htmlHelper.ViewData.ModelState.Keys.Where(expression))
                {
                    foreach (var err in htmlHelper.ViewData.ModelState[key].Errors)
                    {
                        stringRetorno.Append("<div class=\"alert alert-danger fade in\">");
                        stringRetorno.Append("<button class=\"close\" data-dismiss=\"alert\">×</button>");
                        stringRetorno.Append("<i class=\"fa-fw fa fa-times\"></i>\n");
                        stringRetorno.Append(err.ErrorMessage.Replace("\n", "<br />"));
                        stringRetorno.Append("</div>");
                    }
                }
            }
            return stringRetorno.ToString();
        }
    }
}