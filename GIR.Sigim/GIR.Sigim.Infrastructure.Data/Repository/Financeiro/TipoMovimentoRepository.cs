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
    public class TipoMovimentoRepository : Repository<TipoMovimento>, ITipoMovimentoRepository
    {
        #region Construtor

        public TipoMovimentoRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }
        #endregion

        #region IRepository<TEntity> Members

        public override IEnumerable<TipoMovimento> ListarPeloFiltroComPaginacao(
            ISpecification<TipoMovimento> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<TipoMovimento, object>>[] includes)
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
                case "historicoContabil":
                    set = ascending ? set.OrderBy(l => l.HistoricoContabil) : set.OrderByDescending(l => l.HistoricoContabil);
                    break;
                case "tipo":
                    set = ascending ? set.OrderBy(l => l.Tipo) : set.OrderByDescending(l => l.Tipo);
                    break;
                case "operacao":
                    set = ascending ? set.OrderBy(l => l.Operacao) : set.OrderByDescending(l => l.Operacao);
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
