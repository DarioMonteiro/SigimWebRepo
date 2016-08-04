using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Repository.Orcamento;

namespace GIR.Sigim.Infrastructure.Data.Repository.Orcamento
{
    public class EmpresaRepository : Repository<Empresa>, IEmpresaRepository
    {
        #region Constructor

        public EmpresaRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

    }
}
