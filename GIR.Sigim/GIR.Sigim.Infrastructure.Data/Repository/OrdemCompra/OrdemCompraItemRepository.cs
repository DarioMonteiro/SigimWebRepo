using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Repository.OrdemCompra;
using GIR.Sigim.Domain.Specification;

namespace GIR.Sigim.Infrastructure.Data.Repository.OrdemCompra
{
    public class OrdemCompraItemRepository : Repository<OrdemCompraItem>, IOrdemCompraItemRepository
    {
        #region Constructor

        public OrdemCompraItemRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IRepository<OrdemCompraItem> Members

        public override IEnumerable<OrdemCompraItem> ListarPeloFiltroComPaginacao(
            ISpecification<OrdemCompraItem> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<OrdemCompraItem, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            set = set.Where(specification.SatisfiedBy());

            totalRecords = set.Count();

            switch (orderBy)
            {
                case "ordemCompraId":
                    set = ascending ? set.OrderBy(l => l.OrdemCompraId): set.OrderByDescending(l => l.OrdemCompraId);
                    break;
                case "dataOrdemCompra":
                    set = ascending ? set.OrderBy(l => l.OrdemCompra.Data) : set.OrderByDescending(l => l.OrdemCompra.Data);
                    break;
                case "descricaoSituacao":
                    set = ascending ? set.OrderBy(l => l.OrdemCompra.Situacao) : set.OrderByDescending(l => l.OrdemCompra.Situacao);
                    break;
                case "prazoEntrega":
                    set = ascending ? set.OrderBy(l => l.OrdemCompra.PrazoEntrega) : set.OrderByDescending(l => l.OrdemCompra.PrazoEntrega);
                    break;
                case "nomeFornecedor":
                    set = ascending ? set.OrderBy(l => l.OrdemCompra.ClienteFornecedor.Nome) : set.OrderByDescending(l => l.OrdemCompra.ClienteFornecedor.Nome);
                    break;
                case "materialId":
                    set = ascending ? set.OrderBy(l => l.MaterialId) : set.OrderByDescending(l => l.MaterialId);
                    break;
                case "descricaoMaterial":
                    set = ascending ? set.OrderBy(l => l.Material.Descricao) : set.OrderByDescending(l => l.Material.Descricao);
                    break;
                case "complementoDescricao":
                    set = ascending ? set.OrderBy(l => l.Complemento) : set.OrderByDescending(l => l.Complemento);
                    break;
                case "unidadeMedida":
                    set = ascending ? set.OrderBy(l => l.Material.SiglaUnidadeMedida) : set.OrderByDescending(l => l.Material.SiglaUnidadeMedida);
                    break;
                case "codigoClasse":
                    set = ascending ? set.OrderBy(l => l.Classe.Codigo) : set.OrderByDescending(l => l.Classe.Codigo);
                    break;
                case "quantidade":
                    set = ascending ? set.OrderBy(l => l.Quantidade) : set.OrderByDescending(l => l.Quantidade);
                    break;
                case "valorUnitario":
                    set = ascending ? set.OrderBy(l => l.ValorUnitario) : set.OrderByDescending(l => l.ValorUnitario);
                    break;
                case "percentualIPI":
                    set = ascending ? set.OrderBy(l => l.PercentualIPI) : set.OrderByDescending(l => l.PercentualIPI);
                    break;
                case "valorTotalComImposto":
                    set = ascending ? set.OrderBy(l => l.ValorTotalComImposto) : set.OrderByDescending(l => l.ValorTotalComImposto);
                    break;
                case "quantidadeEntregue":
                    set = ascending ? set.OrderBy(l => l.QuantidadeEntregue) : set.OrderByDescending(l => l.QuantidadeEntregue);
                    break;
                //case "saldo":
                //    set = ascending ? set.OrderBy(l => l.Saldo) : set.OrderByDescending(l => l.Saldo);
                //    break;
                default:
                    set = ascending ? set.OrderBy(l => l.OrdemCompraId) : set.OrderByDescending(l => l.OrdemCompraId);
                    break;
            }

            return set.Skip(pageCount * pageIndex).Take(pageCount);
        }

        #endregion


    }
}
