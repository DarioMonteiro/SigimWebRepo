using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Domain.Specification;

namespace GIR.Sigim.Infrastructure.Data.Repository.Sigim
{
    public class ComplementoNaturezaOperacaoRepository : Repository<ComplementoNaturezaOperacao>, IComplementoNaturezaOperacaoRepository
    {
        #region Constructor

        public ComplementoNaturezaOperacaoRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion
    }
}