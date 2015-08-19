using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Repository.Financeiro
{
    public interface ITituloPagarRepository : IRepository<TituloPagar>
    {
        void RemoverTituloPagarAdiantamento(TituloPagarAdiantamento titulo);
        void RemoverImpostoPagar(ImpostoPagar impostoPagar);
    }
}