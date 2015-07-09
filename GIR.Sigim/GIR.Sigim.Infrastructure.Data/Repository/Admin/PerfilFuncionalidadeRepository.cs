using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Admin;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Domain.Specification;

namespace GIR.Sigim.Infrastructure.Data.Repository.Admin
{
    public class PerfilFuncionalidadeRepository : Repository<PerfilFuncionalidade>, IPerfilFuncionalidadeRepository
    {
        #region Constructor

        public PerfilFuncionalidadeRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IPerfilRepository Members

        #endregion
    }
}