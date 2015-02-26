using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Repository.Orcamento;

namespace GIR.Sigim.Infrastructure.Data.Repository.Orcamento
{
    public class ParametrosOrcamentoRepository : Repository<ParametrosOrcamento>, IParametrosOrcamentoRepository
    {
        #region Constructor

        public ParametrosOrcamentoRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion
    }
}