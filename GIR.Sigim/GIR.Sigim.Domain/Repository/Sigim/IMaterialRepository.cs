using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Specification;

namespace GIR.Sigim.Domain.Repository.Sigim
{
    public interface IMaterialRepository : IRepository<Material>
    {
        IEnumerable<Material> Pesquisar(
            ISpecification<Material> specification,
            string orderBy,
            bool ascending,
            params Expression<Func<Material, object>>[] includes);
    }
}