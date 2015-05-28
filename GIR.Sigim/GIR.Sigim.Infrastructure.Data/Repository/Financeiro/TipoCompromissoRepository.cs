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
    public class TipoCompromissoRepository : Repository<TipoCompromisso>, ITipoCompromissoRepository
    {
        #region Constructor

        public TipoCompromissoRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IRepository<TEntity> Members

        public override IEnumerable<TipoCompromisso > ListarPeloFiltroComPaginacao(
            ISpecification<TipoCompromisso> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<TipoCompromisso, object>>[] includes)
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
                case "tipoPagar":
                    set = ascending ? set.OrderBy(l => l.TipoPagar) : set.OrderByDescending(l => l.TipoPagar);
                    break;
                case "tipoReceber":
                    set = ascending ? set.OrderBy(l => l.TipoReceber) : set.OrderByDescending(l => l.TipoReceber);
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