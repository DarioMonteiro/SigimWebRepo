using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Repository.OrdemCompra;

namespace GIR.Sigim.Infrastructure.Data.Repository.OrdemCompra
{
    public class ParametrosOrdemCompraRepository : Repository<ParametrosOrdemCompra>, IParametrosOrdemCompraRepository
    {
        #region Constructor

        public ParametrosOrdemCompraRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion
    }
}