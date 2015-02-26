using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Specification
{
    public sealed class DirectSpecification<TEntity>
        : Specification<TEntity>
        where TEntity : class
    {
        Expression<Func<TEntity, bool>> matchingCriteria;

        public DirectSpecification(Expression<Func<TEntity, bool>> matchingCriteria)
        {
            if (matchingCriteria == (Expression<Func<TEntity, bool>>)null)
                throw new ArgumentNullException("matchingCriteria");

            this.matchingCriteria = matchingCriteria;
        }

        public override Expression<Func<TEntity, bool>> SatisfiedBy()
        {
            return this.matchingCriteria;
        }
    }
}