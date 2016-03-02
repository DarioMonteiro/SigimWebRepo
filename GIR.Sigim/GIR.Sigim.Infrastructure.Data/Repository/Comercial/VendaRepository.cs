using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Comercial;
using GIR.Sigim.Domain.Repository.Comercial;
using GIR.Sigim.Domain.Specification;
using System.Linq.Expressions;

namespace GIR.Sigim.Infrastructure.Data.Repository.Comercial
{
    public class VendaRepository : Repository<Venda>, IVendaRepository
    {
        #region Constructor

        public VendaRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IVendaRepository Members

        public override IEnumerable<Venda> ListarPeloFiltroComPaginacao(
            ISpecification<Venda> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<Venda, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            set = set.Where(specification.SatisfiedBy());

            totalRecords = set.Count();

            switch (orderBy)
            {
                case "id":
                    set = ascending ? set.OrderBy(l => l.Id) : set.OrderByDescending(l => l.Id);
                    break;
                case "contrato":
                    set = ascending ? set.OrderBy(l => l.ContratoId) : set.OrderByDescending(l => l.ContratoId);
                    break;
                case "codigo":
                default:
                    set = ascending ? set.OrderBy(l => l.Id) : set.OrderByDescending(l => l.Id);
                    break;
            }

            return set.Skip(pageCount * pageIndex).Take(pageCount);
        }

        #endregion
    }
}