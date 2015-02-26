using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Repository.Sigim
{
    public class MaterialRepository : Repository<Material>, IMaterialRepository
    {
        #region Constructor

        public MaterialRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IMaterialRepository Members

        public IEnumerable<Material> ListarAtivosPeloTipoTabelaPropria(string descricao, params Expression<Func<Material, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            if (!string.IsNullOrEmpty(descricao))
                set = set.Where(l => l.Descricao.Contains(descricao));

            //TODO: Alterar no banco os materiais com situação == NULL para "A", para simplificar a consulta.
            return set.Where(l => l.TipoTabela == TipoTabela.Propria && (l.Situacao == "A" || string.IsNullOrEmpty(l.Situacao)))
                .OrderBy(l => l.Descricao);
        }

        #endregion
    }
}