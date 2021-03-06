﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Financeiro;

namespace GIR.Sigim.Infrastructure.Data.Repository.Financeiro
{
    public class ImpostoReceberRepository : Repository<ImpostoReceber>, IImpostoReceberRepository
    {
        #region Constructor

        public ImpostoReceberRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IImpostoReceberRepository

        public bool RemoverObjeto(ImpostoReceber impostoReceber)
        {
            try
            {
                this.DeletarObjeto(impostoReceber);
                return true;
            }
            catch (InvalidOperationException ex)
            {
                return false;
            }
        }

        #endregion

    }
}
