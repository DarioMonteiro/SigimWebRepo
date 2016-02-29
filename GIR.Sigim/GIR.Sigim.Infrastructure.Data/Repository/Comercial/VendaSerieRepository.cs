using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Comercial;
using GIR.Sigim.Domain.Repository.Comercial;


namespace GIR.Sigim.Infrastructure.Data.Repository.Comercial
{
    public class VendaSerieRepository : Repository<VendaSerie>, IVendaSerieRepository
    {
        #region Constructor

        public VendaSerieRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IRepository<VendaSerie> Metodos

        public VendaSerie ObterPeloIdComposto(int? id1, int? id2, params Expression<Func<VendaSerie, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            return set.Where(l => (l.ContratoId == id1 && l.NumeroSerie == id2)).SingleOrDefault();
        }

        #endregion
    }
}
