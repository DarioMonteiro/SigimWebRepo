using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Comercial;
using GIR.Sigim.Domain.Repository.Comercial;

namespace GIR.Sigim.Infrastructure.Data.Repository.Comercial
{
    public class IncorporadorRepository : Repository<Incorporador>, IIncorporadorRepository
    {
        #region Constructor

        public IncorporadorRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IIncorporadorRepository Members

        #endregion
    }
}