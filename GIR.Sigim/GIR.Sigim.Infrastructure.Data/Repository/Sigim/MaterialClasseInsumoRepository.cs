using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Repository.Sigim
{
    public class MaterialClasseInsumoRepository : Repository<MaterialClasseInsumo>, IMaterialClasseInsumoRepository
    {
        #region Constructor

        public MaterialClasseInsumoRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }
        #endregion

        #region IMaterialClasseInsumoRepository Members

        public MaterialClasseInsumo ObterPeloCodigo(string codigo, params Expression<Func<MaterialClasseInsumo, object>>[] includes)
        {
            var set = QueryableUnitOfWork.CreateSet<MaterialClasseInsumo>().AsQueryable<MaterialClasseInsumo>();

            if (includes.Any())
                set = includes.Aggregate(set, (current, expression) => current.Include(expression));

            return set.Where(l => l.Codigo == codigo).SingleOrDefault();
        }

        public IEnumerable<MaterialClasseInsumo> ListarRaizes()
        {
            return QueryableUnitOfWork.CreateSet<MaterialClasseInsumo>()
                .Where(l => l.ClassePai == null);
        }

        #endregion

    }
}
