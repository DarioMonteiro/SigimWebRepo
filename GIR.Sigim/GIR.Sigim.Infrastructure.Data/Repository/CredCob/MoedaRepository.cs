using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.CredCob;
using GIR.Sigim.Domain.Repository.CredCob;

namespace GIR.Sigim.Infrastructure.Data.Repository.CredCob
{
    public class MoedaRepository : Repository<Moeda>, IMoedaRepository
    {
        #region Constructor

        public MoedaRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

    }
}
