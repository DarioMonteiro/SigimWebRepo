using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sac;
using GIR.Sigim.Domain.Repository.Sac;

namespace GIR.Sigim.Infrastructure.Data.Repository.Sac
{
    public class ParametrosEmailSacRepository : Repository<ParametrosEmailSac>, IParametrosEmailSacRepository
    {
        #region Constructor

        public ParametrosEmailSacRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        #endregion

        #region IParametrosSacRepository Members

       

        #endregion
    }
}