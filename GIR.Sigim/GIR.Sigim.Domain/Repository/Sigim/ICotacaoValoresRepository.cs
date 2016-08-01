using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Repository.Sigim
{
    public interface ICotacaoValoresRepository : IRepository<CotacaoValores> 
    {
        decimal RecuperaCotacao(int indiceId, DateTime data);
        CotacaoValores ObtemCotacao(int indiceId, DateTime data);
    }
}
