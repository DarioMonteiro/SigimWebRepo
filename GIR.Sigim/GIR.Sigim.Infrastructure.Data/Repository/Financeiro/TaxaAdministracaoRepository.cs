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
    public class TaxaAdministracaoRepository : Repository<TaxaAdministracao>, ITaxaAdministracaoRepository
    {
        #region Constructor

        public TaxaAdministracaoRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IRepository<TEntity> Members

        public override IEnumerable<TaxaAdministracao> ListarPeloFiltroComPaginacao(
            ISpecification<TaxaAdministracao> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<TaxaAdministracao, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            set = set.Where(specification.SatisfiedBy());
            
            //set = set.Select(s => new TaxaAdministracao {CentroCustoId = s.CentroCustoId, ClienteId = s.ClienteId}).Distinct();
            //set = set.Select(s => new { s.CentroCustoId, s.ClienteId });

            totalRecords = set.Count();

            switch (orderBy)
            {
                case "id":
                    set = ascending ? set.OrderBy(l => l.Id) : set.OrderByDescending(l => l.Id);
                    break;
                case "centroCusto":
                    set = ascending ? set.OrderBy(l => l.CentroCustoId) : set.OrderByDescending(l => l.CentroCustoId);
                    break;
                case "cliente":
                    set = ascending ? set.OrderBy(l => l.ClienteId) : set.OrderByDescending(l => l.ClienteId);
                    break;
                default:
                    set = ascending ? set.OrderBy(l => l.Id) : set.OrderByDescending(l => l.Id);
                    break;
            }



            return set.Skip(pageCount * pageIndex).Take(pageCount);
        }

        #endregion
    }
}
