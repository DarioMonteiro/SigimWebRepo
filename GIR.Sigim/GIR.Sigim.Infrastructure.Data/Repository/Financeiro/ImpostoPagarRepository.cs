using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Financeiro;

namespace GIR.Sigim.Infrastructure.Data.Repository.Financeiro
{
    public class ImpostoPagarRepository : Repository<ImpostoPagar>, IImpostoPagarRepository
    {
        #region Constructor

        public ImpostoPagarRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

    }
}
