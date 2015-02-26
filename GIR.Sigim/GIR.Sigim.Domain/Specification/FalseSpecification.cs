using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Specification
{
    public sealed class FalseSpecification<TEntity>
        : Specification<TEntity>
        where TEntity : class
    {
        public override Expression<Func<TEntity, bool>> SatisfiedBy()
        {
            bool result = false;

            Expression<Func<TEntity, bool>> falseExpression = t => result;
            return falseExpression;
        }
    }
}