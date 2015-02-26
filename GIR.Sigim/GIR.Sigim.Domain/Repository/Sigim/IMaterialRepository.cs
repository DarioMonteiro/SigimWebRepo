using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Repository.Sigim
{
    public interface IMaterialRepository : IRepository<Material>
    {
        IEnumerable<Material> ListarAtivosPeloTipoTabelaPropria(string descricao, params Expression<Func<Material, object>>[] includes);
    }
}