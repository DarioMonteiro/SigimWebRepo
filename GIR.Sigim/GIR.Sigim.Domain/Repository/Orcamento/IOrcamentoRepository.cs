using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity;

namespace GIR.Sigim.Domain.Repository.Orcamento
{
    public interface IOrcamentoRepository : IRepository<Entity.Orcamento.Orcamento>
    {
        Domain.Entity.Orcamento.Orcamento ObterUltimoOrcamentoPeloCentroCusto(string codigoCentroCusto);
        Domain.Entity.Orcamento.Orcamento ObterUltimoOrcamentoPeloCentroCustoClasseOrcamento(string codigoCentroCusto);
    }
}