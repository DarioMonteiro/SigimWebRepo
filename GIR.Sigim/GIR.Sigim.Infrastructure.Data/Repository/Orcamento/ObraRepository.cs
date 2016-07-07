using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Repository.Orcamento;

namespace GIR.Sigim.Infrastructure.Data.Repository.Orcamento
{
    public class ObraRepository : Repository<Obra>, IObraRepository
    {
        #region Constructor

        public ObraRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

    }
}
