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
    public class ContaCorrenteRepository : Repository<ContaCorrente>, IContaCorrenteRepository
    {
        #region Constructor

        public ContaCorrenteRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IRepository<TEntity> Members

        public override IEnumerable<ContaCorrente> ListarPeloFiltroComPaginacao(
            ISpecification<ContaCorrente> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<ContaCorrente, object>>[] includes)
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
                case "agenciaId":
                    set = ascending ? set.OrderBy(l => l.AgenciaId) : set.OrderByDescending(l => l.AgenciaId);
                    break;
                case "contaCodigo":
                    set = ascending ? set.OrderBy(l => l.ContaCodigo) : set.OrderByDescending(l => l.ContaCodigo);
                    break;    
                case "DVConta":
                    set = ascending ? set.OrderBy(l => l.DVConta) : set.OrderByDescending(l => l.DVConta);
                    break;
                case "codigoEmpresa":
                    set = ascending ? set.OrderBy(l => l.CodigoEmpresa) : set.OrderByDescending(l => l.CodigoEmpresa);
                    break;
                case "nomeCedente":
                    set = ascending ? set.OrderBy(l => l.NomeCedente) : set.OrderByDescending(l => l.NomeCedente);
                    break;
                case "CNPJCedente":
                    set = ascending ? set.OrderBy(l => l.CNPJCedente) : set.OrderByDescending(l => l.CNPJCedente);
                    break;
                case "descricao":
                    set = ascending ? set.OrderBy(l => l.Descricao) : set.OrderByDescending(l => l.Descricao);
                    break;
                case "complemento":
                    set = ascending ? set.OrderBy(l => l.Complemento) : set.OrderByDescending(l => l.Complemento);
                    break;
                case "tipo":
                    set = ascending ? set.OrderBy(l => l.Tipo) : set.OrderByDescending(l => l.Tipo);
                    break;
                case "situacao":
                    set = ascending ? set.OrderBy(l => l.Situacao) : set.OrderByDescending(l => l.Situacao);
                    break;
                case "codigo":
                default:
                    set = ascending ? set.OrderBy(l => l.BancoId) : set.OrderByDescending(l => l.BancoId);
                    break;
            }

            return set.Skip(pageCount * pageIndex).Take(pageCount);
        }

        
        #endregion
    }
}