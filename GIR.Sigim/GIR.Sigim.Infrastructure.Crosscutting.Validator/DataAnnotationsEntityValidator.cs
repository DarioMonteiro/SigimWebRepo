using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Infrastructure.Crosscutting.Validator
{
    public class DataAnnotationsEntityValidator : IEntityValidator
    {
        public bool IsValid<TEntity>(TEntity entity, out List<String> validationErrors) where TEntity : class
        {
            validationErrors = new List<string>();

            if (entity == null)
                return false;

            SetValidatableObjectErrors(entity, validationErrors);

            return !validationErrors.Any();
        }

        void SetValidatableObjectErrors<TEntity>(TEntity item, List<string> errors) where TEntity : class
        {
            if (typeof(IValidatableObject).IsAssignableFrom(typeof(TEntity)))
            {
                var validationContext = new ValidationContext(item, null, null);

                var validationResults = ((IValidatableObject)item).Validate(validationContext);

                errors.AddRange(validationResults.Select(vr => vr.ErrorMessage));
            }
        }
    }
}