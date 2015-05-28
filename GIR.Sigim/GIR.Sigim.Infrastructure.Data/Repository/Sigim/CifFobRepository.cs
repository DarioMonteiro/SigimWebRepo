using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Repository.Sigim
{
    public class CifFobRepository : Repository<CifFob>, ICifFobRepository
    {
        #region Construtor

        public CifFobRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion
    }
}
