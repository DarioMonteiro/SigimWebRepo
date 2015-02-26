using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity;

namespace GIR.Sigim.Domain.Specification
{
    public class BaseSpecification<TEntity> where TEntity : BaseEntity
    {
        public static Specification<TEntity> MatchingId(int? id)
        {
            Specification<TEntity> specification = new TrueSpecification<TEntity>();

            if (id.HasValue)
            {
                var idSpecification = new DirectSpecification<TEntity>(l => l.Id == id);
                specification &= idSpecification;
            }

            return specification;
        }
    }
}