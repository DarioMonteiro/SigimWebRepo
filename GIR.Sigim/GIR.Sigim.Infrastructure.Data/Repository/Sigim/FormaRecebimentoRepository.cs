using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim ;
using GIR.Sigim.Domain.Repository.Sigim ;
using GIR.Sigim.Domain.Specification;


namespace GIR.Sigim.Infrastructure.Data.Repository.Sigim
{
    public class FormaRecebimentoRepository : Repository<FormaRecebimento>, IFormaRecebimentoRepository
    {
        #region Constructor

        public FormaRecebimentoRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IRepository<TEntity> Members

        public override IEnumerable<FormaRecebimento> ListarPeloFiltroComPaginacao(
            ISpecification<FormaRecebimento> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<FormaRecebimento, object>>[] includes)
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
                case "tipoRecebimento":
                    set = ascending ? set.OrderBy(l => l.TipoRecebimento) : set.OrderByDescending(l => l.Descricao);
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