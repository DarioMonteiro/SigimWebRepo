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
    public class NaturezaReceitaRepository : Repository<NaturezaReceita>, INaturezaReceitaRepository
    {
        #region Constructor

        public NaturezaReceitaRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion
    }
}