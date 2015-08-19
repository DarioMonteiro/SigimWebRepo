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
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<Material, object>>[] includes);
    }
}