﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Repository.Contrato;

namespace GIR.Sigim.Infrastructure.Data.Repository.Contrato
{
    public class ContratoRetencaoRepository : Repository<ContratoRetencao>, IContratoRetencaoRepository
    {

        #region Constructor

        public ContratoRetencaoRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion
    }
}
