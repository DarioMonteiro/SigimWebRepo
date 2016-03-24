using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Comercial;
using GIR.Sigim.Domain.Repository.Comercial;
using GIR.Sigim.Domain.Specification;

namespace GIR.Sigim.Infrastructure.Data.Repository.Comercial
{
    public class BlocoRepository : Repository<Bloco>, IBlocoRepository
    {
        #region Constructor

        public BlocoRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IBlocoRepository Members

        public List<Bloco> ListarPeloEmpreendimento(int empreendimentoId)
        {
            return QueryableUnitOfWork.CreateSet<Bloco>().Where(l => l.EmpreendimentoId == empreendimentoId).ToList<Bloco>();
        }

        #endregion
    }
}