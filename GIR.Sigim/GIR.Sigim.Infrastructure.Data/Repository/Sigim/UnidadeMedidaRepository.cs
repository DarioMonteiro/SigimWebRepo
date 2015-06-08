using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Domain.Specification;

namespace GIR.Sigim.Infrastructure.Data.Repository.Sigim
{
    public class UnidadeMedidaRepository : Repository<UnidadeMedida>, IUnidadeMedidaRepository
    {
        #region Constructor

        public UnidadeMedidaRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IRepository<TEntity> Members

        public override IEnumerable<UnidadeMedida> ListarPeloFiltroComPaginacao(
            ISpecification<UnidadeMedida> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<UnidadeMedida, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            set = set.Where(specification.SatisfiedBy());

            totalRecords = set.Count();

            switch (orderBy)
            {
                case "descricao":
                    set = ascending ? set.OrderBy(l => l.Descricao) : set.OrderByDescending(l => l.Descricao);
                    break;
                case "sigla":
                default:
                    set = ascending ? set.OrderBy(l => l.Sigla) : set.OrderByDescending(l => l.Sigla);
                    break;
            }

            return set.Skip(pageCount * pageIndex).Take(pageCount);
        }

        public UnidadeMedida ObterPeloCodigo(string sigla, params Expression<Func<UnidadeMedida, object>>[] includes)
        {
            var set = QueryableUnitOfWork.CreateSet<UnidadeMedida>().AsQueryable<UnidadeMedida>();

            if (includes.Any())
                set = includes.Aggregate(set, (current, expression) => current.Include(expression));


            return set.Where(l => l.Sigla == sigla).SingleOrDefault();
        }

        
        #endregion
    }
}