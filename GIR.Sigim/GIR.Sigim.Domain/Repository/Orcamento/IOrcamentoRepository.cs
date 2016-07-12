using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity;
using GIR.Sigim.Domain.Specification;

namespace GIR.Sigim.Domain.Repository.Orcamento
{
    public interface IOrcamentoRepository : IRepository<Entity.Orcamento.Orcamento>
    {
        Domain.Entity.Orcamento.Orcamento ObterPrimeiroOrcamentoPeloCentroCusto(string codigoCentroCusto, params Expression<Func<Domain.Entity.Orcamento.Orcamento, object>>[] includes);
        Domain.Entity.Orcamento.Orcamento ObterUltimoOrcamentoPeloCentroCusto(string codigoCentroCusto, params Expression<Func<Domain.Entity.Orcamento.Orcamento, object>>[] includes);
        Domain.Entity.Orcamento.Orcamento ObterUltimoOrcamentoPeloCentroCustoClasseOrcamento(string codigoCentroCusto);
        IEnumerable<Domain.Entity.Orcamento.Orcamento> Pesquisar(ISpecification<Domain.Entity.Orcamento.Orcamento> specification,
                                                               int pageIndex,
                                                               int pageCount,
                                                               string orderBy,
                                                               bool ascending,
                                                               out int totalRecords,
                                                               params Expression<Func<Domain.Entity.Orcamento.Orcamento, object>>[] includes);

    }
}