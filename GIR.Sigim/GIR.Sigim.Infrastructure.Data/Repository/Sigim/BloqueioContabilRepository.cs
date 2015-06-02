using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Repository.Sigim
{
    public class BloqueioContabilRepository : Repository<BloqueioContabil>,IBloqueioContabilRepository
    {

        #region Construtor

        public BloqueioContabilRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IBloqueioContabilRepository Membros
        #endregion
    }
}
