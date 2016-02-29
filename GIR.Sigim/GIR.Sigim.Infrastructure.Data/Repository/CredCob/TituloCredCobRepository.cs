using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.CredCob;
using GIR.Sigim.Domain.Repository.CredCob;


namespace GIR.Sigim.Infrastructure.Data.Repository.CredCob
{
    public class TituloCredCobRepository : Repository<TituloCredCob>, ITituloCredCobRepository
    {
        #region Constructor

        public TituloCredCobRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

    }
}
