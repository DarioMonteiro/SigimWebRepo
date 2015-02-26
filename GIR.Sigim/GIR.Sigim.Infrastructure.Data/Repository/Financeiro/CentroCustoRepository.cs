using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Financeiro;

namespace GIR.Sigim.Infrastructure.Data.Repository.Financeiro
{
    public class CentroCustoRepository : Repository<CentroCusto>, ICentroCustoRepository
    {
        #region Constructor

        public CentroCustoRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region ICentroCustoRepository Members

        public CentroCusto ObterPeloCodigo(string codigo, params Expression<Func<CentroCusto, object>>[] includes)
        {
            var set = QueryableUnitOfWork.CreateSet<CentroCusto>().AsQueryable<CentroCusto>();

            if (includes.Any())
                set = includes.Aggregate(set, (current, expression) => current.Include(expression));

            return set.Where(l => l.Codigo == codigo).SingleOrDefault();
        }

        public IEnumerable<CentroCusto> ListarRaizesAtivas()
        {
            return QueryableUnitOfWork.CreateSet<CentroCusto>()
                .Where(l => l.CentroCustoPai == null && l.Situacao == "A");
        }

        #endregion
    }
}