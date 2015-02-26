using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Specification
{
    public sealed class OrSpecification<T>
         : CompositeSpecification<T>
         where T : class
    {
        private ISpecification<T> rightSideSpecification = null;
        private ISpecification<T> leftSideSpecification = null;

        public OrSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
        {
            if (leftSide == (ISpecification<T>)null)
                throw new ArgumentNullException("leftSide");

            if (rightSide == (ISpecification<T>)null)
                throw new ArgumentNullException("rightSide");

            this.leftSideSpecification = leftSide;
            this.rightSideSpecification = rightSide;
        }

        public override ISpecification<T> LeftSideSpecification
        {
            get { return leftSideSpecification; }
        }

        public override ISpecification<T> RightSideSpecification
        {
            get { return rightSideSpecification; }
        }

        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            Expression<Func<T, bool>> left = leftSideSpecification.SatisfiedBy();
            Expression<Func<T, bool>> right = rightSideSpecification.SatisfiedBy();

            return (left.Or(right));
        }
    }
}