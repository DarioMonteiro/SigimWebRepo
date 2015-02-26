using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Repository.OrdemCompra;

namespace GIR.Sigim.Infrastructure.Data.Repository.OrdemCompra
{
    public class ParametrosUsuarioRepository : Repository<ParametrosUsuario>, IParametrosUsuarioRepository
    {
        #region Constructor

        public ParametrosUsuarioRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion
    }
}