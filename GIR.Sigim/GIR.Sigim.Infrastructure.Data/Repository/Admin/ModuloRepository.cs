using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Admin;
using GIR.Sigim.Domain.Repository.Admin;

namespace GIR.Sigim.Infrastructure.Data.Repository.Admin
{
    public class ModuloRepository : Repository<Modulo>, IModuloRepository
    {
        #region Constructor

        public ModuloRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IModuloRepository Members

        #endregion
    }
}