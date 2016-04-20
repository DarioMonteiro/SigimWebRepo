using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Repository.Orcamento;

namespace GIR.Sigim.Infrastructure.Data.Repository.Orcamento
{
    public class OrcamentoRepository : Repository<Domain.Entity.Orcamento.Orcamento>, IOrcamentoRepository
    {
        #region Constructor

        public OrcamentoRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IOrcamentoRepository Members

        public Domain.Entity.Orcamento.Orcamento ObterPrimeiroOrcamentoPeloCentroCusto(string codigoCentroCusto, params Expression<Func<Domain.Entity.Orcamento.Orcamento, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            set = set.Where(l => l.Situacao == "A"
                && l.Obra.CodigoCentroCusto == codigoCentroCusto);

            set = set.OrderBy(l => l.Sequencial);
            return set.FirstOrDefault();
        }

        public Domain.Entity.Orcamento.Orcamento ObterUltimoOrcamentoPeloCentroCusto(string codigoCentroCusto, params Expression<Func<Domain.Entity.Orcamento.Orcamento, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            set = set.Where(l => l.Situacao == "A"
                && l.Obra.CodigoCentroCusto == codigoCentroCusto);

            set = set.OrderByDescending(l => l.Sequencial);
            return set.FirstOrDefault();
        }

        public Domain.Entity.Orcamento.Orcamento ObterUltimoOrcamentoPeloCentroCustoClasseOrcamento(string codigoCentroCusto)
        {
            var set = CreateSetAsQueryable();

            set = set.Where(l => l.Situacao == "A"
                && l.Obra.CentroCusto.Codigo == codigoCentroCusto
                && l.Obra.CentroCusto.ListaCentroCustoEmpresa.Any(s => s.EhClasseOrcamento.Value));

            set = set.OrderByDescending(l => l.Sequencial);
            return set.FirstOrDefault();
        }

        #endregion
    }
}