using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Domain.Repository.OrdemCompra
{
    public interface IEntradaMaterialRepository : IRepository<EntradaMaterial>
    {
        void RemoverImpostoPagar(ImpostoPagar impostoPagar);
        void RemoverEntradaMaterialItem(EntradaMaterialItem entradaMaterialItem);
        void RemoverEntradaMaterialImposto(EntradaMaterialImposto EntradaMaterialImposto);
        void RemoverEntradaMaterialFormaPagamento(EntradaMaterialFormaPagamento entradaMaterialFormaPagamento);
    }
}