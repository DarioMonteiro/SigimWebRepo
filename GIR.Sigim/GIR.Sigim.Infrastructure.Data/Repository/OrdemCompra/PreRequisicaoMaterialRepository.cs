using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Repository.OrdemCompra;
using GIR.Sigim.Domain.Specification;

namespace GIR.Sigim.Infrastructure.Data.Repository.OrdemCompra
{
    public class PreRequisicaoMaterialRepository : Repository<PreRequisicaoMaterial>, IPreRequisicaoMaterialRepository
    {
        #region Constructor

        public PreRequisicaoMaterialRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IRepository<TEntity> Members

        public override IEnumerable<PreRequisicaoMaterial> ListarPeloFiltroComPaginacao(
            ISpecification<PreRequisicaoMaterial> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<PreRequisicaoMaterial, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            set = set.Where(specification.SatisfiedBy());

            totalRecords = set.Count();

            switch (orderBy)
            {
                case "situacao":
                    set = ascending ? set.OrderBy(l => l.Situacao) : set.OrderByDescending(l => l.Situacao);
                    break;
                case "data":
                    set = ascending ? set.OrderBy(l => l.Data) : set.OrderByDescending(l => l.Data);
                    break;
                case "observacao":
                    set = ascending ? set.OrderBy(l => l.Observacao) : set.OrderByDescending(l => l.Observacao);
                    break;
                //case "rmgeradas":
                //    set = set.OrderByDescending(s => s.ListaItens.SelectMany(l => l.ListaRequisicaoMaterialItem.Select(c => c.RequisicaoMaterialId)));
                //    break;
                case "responsavel":
                    set = ascending ? set.OrderBy(l => l.LoginUsuarioCadastro) : set.OrderByDescending(l => l.LoginUsuarioCadastro);
                    break;
                case "codigo":
                default:
                    set = ascending ? set.OrderBy(l => l.Id) : set.OrderByDescending(l => l.Id);
                    break;
            }

            return set.Skip(pageCount * pageIndex).Take(pageCount);
        }

        public void RemoverItem(PreRequisicaoMaterialItem item)
        {
            if (item != (PreRequisicaoMaterialItem)null)
            {
                QueryableUnitOfWork.Attach(item);
                QueryableUnitOfWork.CreateSet<PreRequisicaoMaterialItem>().Remove(item);
            }
        }

        #endregion
    }
}