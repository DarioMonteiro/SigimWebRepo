using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Specification
{
    public sealed class NotSpecification<TEntity>
        : Specification<TEntity>
        where TEntity : class
    {
        Expression<Func<TEntity, bool>> originalCriteria;

        public NotSpecification(ISpecification<TEntity> originalSpecification)
        {
            if (originalSpecification == (ISpecification<TEntity>)null)
                throw new ArgumentNullException("originalSpecification");

            originalCriteria = originalSpecification.SatisfiedBy();
        }

        public NotSpecification(Expression<Func<TEntity, bool>> originalSpecification)
        {
            if (originalSpecification == (Expression<Func<TEntity, bool>>)null)
                throw new ArgumentNullException("originalSpecification");

            originalCriteria = originalSpecification;
        }

        public override Expression<Func<TEntity, bool>> SatisfiedBy()
        {
            return Expression.Lambda<Func<TEntity, bool>>(Expression.Not(originalCriteria.Body),
                                                         originalCriteria.Parameters.Single());
        }
    }
}