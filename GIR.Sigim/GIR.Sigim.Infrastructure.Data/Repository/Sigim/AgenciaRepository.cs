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
    public class AgenciaRepository : Repository<Agencia>, IAgenciaRepository
    {
        #region Constructor

        public AgenciaRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IRepository<TEntity> Members

        public override IEnumerable<Agencia> ListarPeloFiltroComPaginacao(
            ISpecification<Agencia> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<Agencia, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            set = set.Where(specification.SatisfiedBy());

            totalRecords = set.Count();

            switch (orderBy)
            {
                case "id":
                    set = ascending ? set.OrderBy(l => l.BancoId) : set.OrderByDescending(l => l.BancoId);
                    break;
                case "bancoId":
                    set = ascending ? set.OrderBy(l => l.BancoId) : set.OrderByDescending(l => l.BancoId);
                    break;
                case "bancoNome":
                    set = ascending ? set.OrderBy(l => l.Banco.Nome) : set.OrderByDescending(l => l.Banco.Nome);
                    break;
                case "agenciaCodigo":
                    set = ascending ? set.OrderBy(l => l.AgenciaCodigo) : set.OrderByDescending(l => l.AgenciaCodigo);
                    break;    
                case "DVAgencia":
                    set = ascending ? set.OrderBy(l => l.DVAgencia) : set.OrderByDescending(l => l.DVAgencia);
                    break;
                case "nome":
                    set = ascending ? set.OrderBy(l => l.Nome) : set.OrderByDescending(l => l.Nome);
                    break;
                case "nomeContato":
                    set = ascending ? set.OrderBy(l => l.NomeContato) : set.OrderByDescending(l => l.NomeContato);
                    break;
                case "telefoneContato":
                    set = ascending ? set.OrderBy(l => l.TelefoneContato) : set.OrderByDescending(l => l.TelefoneContato);
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