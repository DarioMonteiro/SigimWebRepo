using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Repository.Contrato;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Infrastructure.Data.Repository.Contrato
{
    public class ContratoRetificacaoItemRepository : Repository<ContratoRetificacaoItem>, IContratoRetificacaoItemRepository 
    {
        #region Construtor

        public ContratoRetificacaoItemRepository(UnitOfWork unitOfWork) 
            : base(unitOfWork)
        {

        }

        #endregion
    }
}
