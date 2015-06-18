using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Repository.OrdemCompra;
using GIR.Sigim.Domain.Specification;

namespace GIR.Sigim.Infrastructure.Data.Repository.OrdemCompra
{
    public class RequisicaoMaterialRepository : Repository<RequisicaoMaterial>, IRequisicaoMaterialRepository
    {
        #region Constructor

        public RequisicaoMaterialRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IRepository<TEntity> Members

        public override IEnumerable<RequisicaoMaterial> ListarPeloFiltroComPaginacao(
            ISpecification<RequisicaoMaterial> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<RequisicaoMaterial, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            set = set.Where(specification.SatisfiedBy());

            totalRecords = set.Count();

            switch (orderBy)
            {
                case "situacao":
                    set = ascending ? set.OrderBy(l => l.Situacao) : set.OrderByDescending(l => l.Situacao);
                    break;
                case "dataRequisicao":
                    set = ascending ? set.OrderBy(l => l.Data) : set.OrderByDescending(l => l.Data);
                    break;
                case "dataAprovacao":
                    set = ascending ? set.OrderBy(l => l.DataAprovacao) : set.OrderByDescending(l => l.DataAprovacao);
                    break;
                case "centroCusto":
                    set = ascending ? set.OrderBy(l => l.CentroCusto.Codigo) : set.OrderByDescending(l => l.CentroCusto.Codigo);
                    break;
                case "observacao":
                    set = ascending ? set.OrderBy(l => l.Observacao) : set.OrderByDescending(l => l.Observacao);
                    break;
                case "motivoCancelamento":
                    set = ascending ? set.OrderBy(l => l.MotivoCancelamento) : set.OrderByDescending(l => l.MotivoCancelamento);
                    break;
                case "id":
                default:
                    set = ascending ? set.OrderBy(l => l.Id) : set.OrderByDescending(l => l.Id);
                    break;
            }

            return set.Skip(pageCount * pageIndex).Take(pageCount);
        }

        public void RemoverItem(RequisicaoMaterialItem item)
        {
            if (item != (RequisicaoMaterialItem)null)
            {
                QueryableUnitOfWork.Attach(item);
                QueryableUnitOfWork.CreateSet<RequisicaoMaterialItem>().Remove(item);
            }
        }

        public void RemoverInsumoRequisitado(OrcamentoInsumoRequisitado insumoRequisitado)
        {
            if (insumoRequisitado != (OrcamentoInsumoRequisitado)null)
            {
                QueryableUnitOfWork.Attach(insumoRequisitado);
                QueryableUnitOfWork.CreateSet<OrcamentoInsumoRequisitado>().Remove(insumoRequisitado);
            }
        }

        #endregion
    }
}