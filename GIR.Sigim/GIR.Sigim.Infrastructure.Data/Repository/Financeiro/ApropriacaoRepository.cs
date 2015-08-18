using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Domain.Specification;

namespace GIR.Sigim.Infrastructure.Data.Repository.Financeiro
{
    public class ApropriacaoRepository : Repository<Apropriacao>, IApropriacaoRepository
    {
        #region Constructor

        public ApropriacaoRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion
    }
}