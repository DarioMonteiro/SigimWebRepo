using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sac;
using GIR.Sigim.Domain.Repository.Sac;

namespace GIR.Sigim.Infrastructure.Data.Repository.Sac
{
    public class ParametrosSacRepository : Repository<ParametrosSac>, IParametrosSacRepository
    {
        #region Constructor

        public ParametrosSacRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IParametrosSacRepository Members

        public ParametrosSac Obter()
        {
            var set = CreateSetAsQueryable(l => l.Cliente, s => s.ListaParametrosEmailSac.Select(c => c.Setor));
            return set.FirstOrDefault();
        }

        #endregion
    }
}