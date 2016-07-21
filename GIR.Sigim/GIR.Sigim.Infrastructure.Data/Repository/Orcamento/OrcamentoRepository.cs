using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Repository.Orcamento;
using GIR.Sigim.Domain.Specification;

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

        public IEnumerable<Domain.Entity.Orcamento.Orcamento> Pesquisar(
            ISpecification<Domain.Entity.Orcamento.Orcamento> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<Domain.Entity.Orcamento.Orcamento, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);
            set = set.Where(specification.SatisfiedBy());
            totalRecords = set.Count();
            set = ConfigurarOrdenacao(set, orderBy, ascending);

            return set.Skip(pageCount * pageIndex).Take(pageCount);
        }


        #endregion

        #region IRepository<Orcamento> Métodos

        private static IQueryable<Domain.Entity.Orcamento.Orcamento> ConfigurarOrdenacao(IQueryable<Domain.Entity.Orcamento.Orcamento> set, string orderBy, bool ascending)
        {
            switch (orderBy)
            {
                case "sequencial":
                    set = ascending ? set.OrderBy(l => l.Sequencial) : set.OrderByDescending(l => l.Sequencial);
                    break;
                case "descricao":
                    set = ascending ? set.OrderBy(l => l.Descricao) : set.OrderByDescending(l => l.Descricao);
                    break;
                case "obra":
                    set = ascending ? set.OrderBy(l => l.Obra.Numero) : set.OrderByDescending(l => l.Obra.Numero);
                    break;
                case "empresa":
                    set = ascending ? set.OrderBy(l => l.Empresa.Numero) : set.OrderByDescending(l => l.Empresa.Numero);
                    break;
                case "centroCusto":
                    set = ascending ? set.OrderBy(l => l.Obra.CodigoCentroCusto) : set.OrderByDescending(l => l.Obra.CodigoCentroCusto);
                    break;
                case "situacao":
                    set = ascending ? set.OrderBy(l => l.Situacao) : set.OrderByDescending(l => l.Situacao);
                    break;
                case "data":
                    set = ascending ? set.OrderBy(l => l.Data) : set.OrderByDescending(l => l.Data);
                    break;
                case "simplificado":
                    set = ascending ? set.OrderBy(l => l.Obra.OrcamentoSimplificado.Value) : set.OrderByDescending(l => l.Obra.OrcamentoSimplificado.Value);
                    break;
                default:
                    set = ascending ? set.OrderBy(l => l.Empresa.Numero).ThenBy(l => l.Obra.Numero).ThenBy(l => l.Sequencial) : set.OrderByDescending(l => l.Empresa.Numero).ThenByDescending(l => l.Obra.Numero).ThenByDescending(l => l.Sequencial);
                    break;
            }
            return set;
        }

        #endregion

    }
}