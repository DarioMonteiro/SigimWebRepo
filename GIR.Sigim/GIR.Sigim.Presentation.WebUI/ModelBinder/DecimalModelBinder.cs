using System;
using System.Globalization;
using System.Web.Mvc;

namespace GIR.Sigim.Presentation.WebUI.ModelBinder
{
    public class DecimalModelBinder : IModelBinder {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            decimal numeroDecimal;
            bool isDecimal = decimal.TryParse(valueResult.AttemptedValue,out numeroDecimal);
            object actualValue = null;

            if (isDecimal)
            {
                ModelState modelState = new ModelState { Value = valueResult };
                try
                {
                    string valor = valueResult.AttemptedValue.Replace(".", "");
                    actualValue = Convert.ToDecimal(valor);
                }
                catch (FormatException e)
                {
                    modelState.Errors.Add(e);
                }

                bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            }
            return actualValue;
        }
    }
}