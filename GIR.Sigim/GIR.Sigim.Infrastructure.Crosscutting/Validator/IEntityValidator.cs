using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Infrastructure.Crosscutting.Validator
{
    public interface IEntityValidator
    {
        bool IsValid<TEntity>(TEntity entity, out List<String> validationErrors)
            where TEntity : class;
    }
}