using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Comercial;

namespace GIR.Sigim.Domain.Repository.Comercial
{
    public interface IVendaSerieRepository : IRepository<VendaSerie> 
    {
        VendaSerie ObterPeloIdComposto(int? id1, int? id2, params Expression<Func<VendaSerie, object>>[] includes);
    }
}
