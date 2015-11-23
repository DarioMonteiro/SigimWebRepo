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
    public class BancoRepository : Repository<Banco>, IBancoRepository
    {
        #region Constructor

        public BancoRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IRepository<TEntity> Members

        public override IEnumerable<Banco> ListarPeloFiltroComPaginacao(
            ISpecification<Banco> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<Banco, object>>[] includes)
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
                    set = ascending ? set.OrderBy(l => l.Nome) : set.OrderByDescending(l => l.Nome);
                    break;
                case "situacao":
                    set = ascending ? set.OrderBy(l => l.Situacao) : set.OrderByDescending(l => l.Situacao);
                    break;
                case "numeroRemessa":
                    set = ascending ? set.OrderBy(l => l.NumeroRemessa) : set.OrderByDescending(l => l.NumeroRemessa);
                    break;
                case "interfaceEletronica":
                    set = ascending ? set.OrderBy(l => l.InterfaceEletronica) : set.OrderByDescending(l => l.InterfaceEletronica);
                    break;
                case "numeroRemessaPagamento":
                    set = ascending ? set.OrderBy(l => l.NumeroRemessaPagamento) : set.OrderByDescending(l => l.NumeroRemessaPagamento);
                    break;
                case "codigoBC":
                default:
                    set = ascending ? set.OrderBy(l => l.Id) : set.OrderByDescending(l => l.Id);
                    break;
            }

            return set.Skip(pageCount * pageIndex).Take(pageCount);
        }

        
        #endregion
    }
}