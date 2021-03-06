﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Financeiro;

namespace GIR.Sigim.Infrastructure.Data.Repository.Financeiro
{
    public class ParametrosUsuarioFinanceiroRepository : Repository<ParametrosUsuarioFinanceiro>, IParametrosUsuarioFinanceiroRepository
    {
        #region Constructor

        public ParametrosUsuarioFinanceiroRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion
    }
}
