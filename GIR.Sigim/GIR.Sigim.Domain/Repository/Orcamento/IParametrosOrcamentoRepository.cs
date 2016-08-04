using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Orcamento;

namespace GIR.Sigim.Domain.Repository.Orcamento
{
    public interface IParametrosOrcamentoRepository : IRepository<ParametrosOrcamento>
    {
        ParametrosOrcamento Obter();
    }
}