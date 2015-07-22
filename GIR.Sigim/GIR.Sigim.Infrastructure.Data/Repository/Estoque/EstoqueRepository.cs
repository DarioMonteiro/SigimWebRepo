using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Estoque;
using GIR.Sigim.Domain.Repository.Estoque;

namespace GIR.Sigim.Infrastructure.Data.Repository.Estoque
{
    public class EstoqueRepository : Repository<Domain.Entity.Estoque.Estoque>, IEstoqueRepository
    {
        #region Constructor

        public EstoqueRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IEstoqueRepository Members

        public Domain.Entity.Estoque.EstoqueMaterial ObterEstoqueMaterialAtivoPeloCentroCustoEMaterial(string codigoCentroCusto,
            int? materialId,
            params System.Linq.Expressions.Expression<Func<EstoqueMaterial, object>>[] includes)
        {
            var set = QueryableUnitOfWork.CreateSet<EstoqueMaterial>().AsQueryable<EstoqueMaterial>();

            if (includes.Any())
                set = includes.Aggregate(set, (current, expression) => current.Include(expression).DefaultIfEmpty());

            set = set.Where(l => l.Estoque.CodigoCentroCusto == codigoCentroCusto
                && l.MaterialId == materialId
                && l.Estoque.Situacao == "A");

            return set.SingleOrDefault();
        }

        #endregion
    }
}