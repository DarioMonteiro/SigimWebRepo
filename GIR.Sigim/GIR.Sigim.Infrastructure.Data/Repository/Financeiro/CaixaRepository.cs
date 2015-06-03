using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Domain.Specification;

namespace GIR.Sigim.Infrastructure.Data.Repository.Financeiro
{
    public class CaixaRepository : Repository<Caixa>, ICaixaRepository
    {
        #region Constructor

        public CaixaRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IRepository<TEntity> Members

        public override IEnumerable<Caixa> ListarPeloFiltroComPaginacao(
            ISpecification<Caixa> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<Caixa, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            set = set.Where(specification.SatisfiedBy());

            totalRecords = set.Count();

            switch (orderBy)
            {
                case "id":
                    set = ascending ? set.OrderBy(l => l.Id) : set.OrderByDescending(l => l.Id);
                    break;
                case "descricao":
                    set = ascending ? set.OrderBy(l => l.Descricao) : set.OrderByDescending(l => l.Descricao);
                    break;
                case "situacao":
                    set = ascending ? set.OrderBy(l => l.Situacao) : set.OrderByDescending(l => l.Situacao);
                    break;
                case "centroContabil":
                    set = ascending ? set.OrderBy(l => l.CentroContabil) : set.OrderByDescending(l => l.CentroContabil);
                    break;
                case "centroCusto":
                    set = ascending ? set.OrderBy(l => l.CentroCusto.Codigo) : set.OrderByDescending(l => l.CentroCusto.Codigo);
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