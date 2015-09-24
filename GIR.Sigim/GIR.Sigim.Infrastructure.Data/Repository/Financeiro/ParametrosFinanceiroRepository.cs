using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Financeiro;

namespace GIR.Sigim.Infrastructure.Data.Repository.Financeiro
{
    public class ParametrosFinanceiroRepository : Repository<ParametrosFinanceiro>, IParametrosFinanceiroRepository
    {
        #region Constructor

        public ParametrosFinanceiroRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IParametrosFinanceiroRepository Members

        public ParametrosFinanceiro Obter()
        {
            var set = CreateSetAsQueryable(l => l.Cliente);
            return set.FirstOrDefault();
        }

        #endregion

    }
}
