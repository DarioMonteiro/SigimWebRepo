using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Specification
{
    public abstract class CompositeSpecification<TEntity>
         : Specification<TEntity>
         where TEntity : class
    {
        public abstract ISpecification<TEntity> LeftSideSpecification { get; }
        public abstract ISpecification<TEntity> RightSideSpecification { get; }
    }
}