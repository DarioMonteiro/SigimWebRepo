using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Repository.Contrato;
using GIR.Sigim.Domain.Specification;

namespace GIR.Sigim.Infrastructure.Data.Repository.Contrato
{
    public class ContratoRepository : Repository<Domain.Entity.Contrato.Contrato>, IContratoRepository
    {
        #region Contructor

        public ContratoRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IRepository<Contrato> Métodos

        public override IEnumerable<Domain.Entity.Contrato.Contrato> ListarPeloFiltroComPaginacao(
            ISpecification<Domain.Entity.Contrato.Contrato> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<Domain.Entity.Contrato.Contrato, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            set = set.Where(specification.SatisfiedBy());

            totalRecords = set.Count();

            switch (orderBy)
            {
                case "centroCusto":
                    set = ascending ? set.OrderBy(l => l.CentroCusto.Codigo) : set.OrderByDescending(l => l.CentroCusto.Codigo);
                    break;
                case "descricaoContrato":
                    set = ascending ? set.OrderBy(l => l.ContratoDescricao.Descricao) : set.OrderByDescending(l => l.ContratoDescricao.Descricao);
                    break;
                case "contratante":
                    set = ascending ? set.OrderBy(l => l.Contratante.Nome) : set.OrderByDescending(l => l.Contratante.Nome);
                    break;
                case "contratado":
                    set = ascending ? set.OrderBy(l => l.Contratado.Nome) : set.OrderByDescending(l => l.Contratado.Nome);
                    break;
                case "dataAssinatura":
                    set = ascending ? set.OrderBy(l => l.DataAssinatura) : set.OrderByDescending(l => l.DataAssinatura);
                    break;
                case "situacao":
                    set = ascending ? set.OrderBy(l => l.Situacao) : set.OrderByDescending(l => l.Situacao);
                    break;
                case "valorContrato":
                    set = ascending ? set.OrderBy(l => l.ValorContrato) : set.OrderByDescending(l => l.ValorContrato);
                    break;
                //case "descricaoMedicaoLiberar":
                //    set = ascending ? set.OrderBy(l => l.TemMedicaoALiberar(l.Id)) : set.OrderByDescending(l => l.TemMedicaoALiberar(l.Id));
                //    break;
                case "id":
                default:
                    set = ascending ? set.OrderBy(l => l.Id) : set.OrderByDescending(l => l.Id);
                    break;
            }

            return set.Skip(pageCount * pageIndex).Take(pageCount);
        }

        public void AdicionarItemMedicao(ContratoRetificacaoItemMedicao item)
        {
            if (item != (ContratoRetificacaoItemMedicao)null)
            {
                QueryableUnitOfWork.CreateSet<ContratoRetificacaoItemMedicao>().Add(item);
            }
        }


        public void RemoverItemMedicao(ContratoRetificacaoItemMedicao item)
        {
            if (item != (ContratoRetificacaoItemMedicao)null)
            {
                QueryableUnitOfWork.Attach(item);
                QueryableUnitOfWork.CreateSet<ContratoRetificacaoItemMedicao>().Remove(item);
            }
        }

        public void AlterarItemMedicao(ContratoRetificacaoItemMedicao item)
        {
            if (item != (ContratoRetificacaoItemMedicao)null)
            {
                QueryableUnitOfWork.SetModified(item);
            }
        }

        public IEnumerable<Domain.Entity.Contrato.Contrato> Pesquisar(
            ISpecification<Domain.Entity.Contrato.Contrato> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<Domain.Entity.Contrato.Contrato, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);
            set = set.Where(specification.SatisfiedBy());
            totalRecords = set.Count();
            set = ConfigurarOrdenacao(set, orderBy, ascending);

            return set.Skip(pageCount * pageIndex).Take(pageCount);
        }

        #endregion

        #region IRepository<Contrato> Métodos

        private static IQueryable<Domain.Entity.Contrato.Contrato> ConfigurarOrdenacao(IQueryable<Domain.Entity.Contrato.Contrato> set, string orderBy, bool ascending)
        {
            switch (orderBy)
            {
                case "centroCusto":
                    set = ascending ? set.OrderBy(l => l.CodigoCentroCusto) : set.OrderByDescending(l => l.CodigoCentroCusto);
                    break;
                case "descricaoContrato":
                    set = ascending ? set.OrderBy(l => l.ContratoDescricao.Descricao) : set.OrderByDescending(l => l.ContratoDescricao.Descricao);
                    break;
                case "contratante":
                    set = ascending ? set.OrderBy(l => l.Contratante.Nome) : set.OrderByDescending(l => l.Contratante.Nome);
                    break;
                case "contratado":
                    set = ascending ? set.OrderBy(l => l.Contratado.Nome) : set.OrderByDescending(l => l.Contratado.Nome);
                    break;
                case "dataAssinatura":
                    set = ascending ? set.OrderBy(l => l.DataAssinatura) : set.OrderByDescending(l => l.DataAssinatura);
                    break;
                case "descricaoSituacao":
                    set = ascending ? set.OrderBy(l => l.Situacao) : set.OrderByDescending(l => l.Situacao);
                    break;
                case "fornecedor":
                    set = ascending ? set.OrderBy(l => l.Contratado.Nome) : set.OrderByDescending(l => l.Contratado.Nome);
                    break;
                case "id":
                default:
                    set = ascending ? set.OrderBy(l => l.Id) : set.OrderByDescending(l => l.Id);
                    break;
            }
            return set;
        }

        #endregion
    }
}
