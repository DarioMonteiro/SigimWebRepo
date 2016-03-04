﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Repository.Sigim
{
    public class ParametrosSigimRepository : Repository<ParametrosSigim>, IParametrosSigimRepository
    {
        #region Constructor

        public ParametrosSigimRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IParametrosSigimRepository Members

        public ParametrosSigim Obter()
        {
            var set = CreateSetAsQueryable();
            return set.FirstOrDefault();
        }

        #endregion

    }
}
