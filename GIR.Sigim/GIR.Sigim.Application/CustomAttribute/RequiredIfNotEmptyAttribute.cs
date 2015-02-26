using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.CustomAttribute
{
    public class RequiredIfNotEmptyAttribute : ValidationAttribute
    {
        string otherPropertyName;

        public RequiredIfNotEmptyAttribute(string otherPropertyName, string errorMessage)
            : base(errorMessage)
        {
            this.otherPropertyName = otherPropertyName;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, otherPropertyName);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success; 
            try
            {
                var otherPropertyInfo = validationContext.ObjectType.GetProperty(this.otherPropertyName);

                var valor = GetDependentPropertyValue(validationContext.ObjectInstance);

                if (!string.IsNullOrEmpty(valor as string) && string.IsNullOrEmpty(value as string))
                    validationResult = new ValidationResult(ErrorMessageString);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return validationResult;
        }

        private object GetDependentPropertyValue(object container)
        {
            var currentType = container.GetType();
            var value = container;

            foreach (string propertyName in otherPropertyName.Split('.'))
            {
                var property = currentType.GetProperty(propertyName);
                value = property.GetValue(value, null);
                currentType = property.PropertyType;
            }

            return value;
        }
    }
}
