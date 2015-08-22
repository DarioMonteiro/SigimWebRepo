using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Specification;


namespace GIR.Sigim.Domain.Repository.OrdemCompra
{
    public interface IOrdemCompraRepository : IRepository<Domain.Entity.OrdemCompra.OrdemCompra>
    {
        IEnumerable<Domain.Entity.OrdemCompra.OrdemCompra> Pesquisar(ISpecification<Domain.Entity.OrdemCompra.OrdemCompra> specification,
                                                                       int pageIndex,
                                                                       int pageCount,
                                                                       string orderBy,
                                                                       bool ascending,
                                                                       out int totalRecords,
                                                                       params Expression<Func<Domain.Entity.OrdemCompra.OrdemCompra, object>>[] includes);

    }
}
