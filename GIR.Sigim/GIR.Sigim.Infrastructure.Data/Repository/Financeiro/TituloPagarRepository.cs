using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Domain.Specification;

namespace GIR.Sigim.Infrastructure.Data.Repository.Financeiro
{
    public class TituloPagarRepository : Repository<TituloPagar>, ITituloPagarRepository
    {

        #region Construtor

        public TituloPagarRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }
        #endregion

        #region ITituloPagarRepository Members

        public void RemoverTituloPagarAdiantamento(TituloPagarAdiantamento titulo)
        {
            if (titulo != (TituloPagarAdiantamento)null)
            {
                QueryableUnitOfWork.Attach(titulo);
                QueryableUnitOfWork.CreateSet<TituloPagarAdiantamento>().Remove(titulo);
            }
        }

        public void RemoverApropriacaoAdiantamento(ApropriacaoAdiantamento apropriacao)
        {
            if (apropriacao != (ApropriacaoAdiantamento)null)
            {
                QueryableUnitOfWork.Attach(apropriacao);
                QueryableUnitOfWork.CreateSet<ApropriacaoAdiantamento>().Remove(apropriacao);
            }
        }

        public void RemoverImpostoPagar(ImpostoPagar impostoPagar)
        {
            if (impostoPagar != (ImpostoPagar)null)
            {
                QueryableUnitOfWork.Attach(impostoPagar);
                QueryableUnitOfWork.CreateSet<ImpostoPagar>().Remove(impostoPagar);
            }
        }

        public void RemoverTituloPagar(TituloPagar titulo)
        {
            if (titulo != (TituloPagar)null)
            {
                QueryableUnitOfWork.Attach(titulo);
                QueryableUnitOfWork.CreateSet<TituloPagar>().Remove(titulo);
            }
        }

        public void RemoverApropriacao(Apropriacao apropriacao)
        {
            if (apropriacao != (Apropriacao)null)
            {
                QueryableUnitOfWork.Attach(apropriacao);
                QueryableUnitOfWork.CreateSet<Apropriacao>().Remove(apropriacao);
            }
        }

        public override IEnumerable<TituloPagar> ListarPeloFiltroComPaginacao(
            ISpecification<TituloPagar> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<TituloPagar, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            set = set.Where(specification.SatisfiedBy());

            totalRecords = set.Count();

            switch (orderBy)
            {
                case "tituloId":
                    set = ascending ? set.OrderBy(l => l.Id) : set.OrderByDescending(l => l.Id);
                    break;
                //case "dataOrdemCompra":
                //    set = ascending ? set.OrderBy(l => l.OrdemCompra.Data) : set.OrderByDescending(l => l.OrdemCompra.Data);
                //    break;
                //case "descricaoSituacao":
                //    set = ascending ? set.OrderBy(l => l.OrdemCompra.Situacao) : set.OrderByDescending(l => l.OrdemCompra.Situacao);
                //    break;
                //case "prazoEntrega":
                //    set = ascending ? set.OrderBy(l => l.OrdemCompra.PrazoEntrega) : set.OrderByDescending(l => l.OrdemCompra.PrazoEntrega);
                //    break;
                //case "nomeFornecedor":
                //    set = ascending ? set.OrderBy(l => l.OrdemCompra.ClienteFornecedor.Nome) : set.OrderByDescending(l => l.OrdemCompra.ClienteFornecedor.Nome);
                //    break;
                //case "materialId":
                //    set = ascending ? set.OrderBy(l => l.MaterialId) : set.OrderByDescending(l => l.MaterialId);
                //    break;
                //case "descricaoMaterial":
                //    set = ascending ? set.OrderBy(l => l.Material.Descricao) : set.OrderByDescending(l => l.Material.Descricao);
                //    break;
                //case "complementoDescricao":
                //    set = ascending ? set.OrderBy(l => l.Complemento) : set.OrderByDescending(l => l.Complemento);
                //    break;
                //case "unidadeMedida":
                //    set = ascending ? set.OrderBy(l => l.Material.SiglaUnidadeMedida) : set.OrderByDescending(l => l.Material.SiglaUnidadeMedida);
                //    break;
                //case "codigoClasse":
                //    set = ascending ? set.OrderBy(l => l.Classe.Codigo) : set.OrderByDescending(l => l.Classe.Codigo);
                //    break;
                //case "quantidade":
                //    set = ascending ? set.OrderBy(l => l.Quantidade) : set.OrderByDescending(l => l.Quantidade);
                //    break;
                //case "valorUnitario":
                //    set = ascending ? set.OrderBy(l => l.ValorUnitario) : set.OrderByDescending(l => l.ValorUnitario);
                //    break;
                //case "percentualIPI":
                //    set = ascending ? set.OrderBy(l => l.PercentualIPI) : set.OrderByDescending(l => l.PercentualIPI);
                //    break;
                //case "valorTotalComImposto":
                //    set = ascending ? set.OrderBy(l => l.ValorTotalComImposto) : set.OrderByDescending(l => l.ValorTotalComImposto);
                //    break;
                //case "quantidadeEntregue":
                //    set = ascending ? set.OrderBy(l => l.QuantidadeEntregue) : set.OrderByDescending(l => l.QuantidadeEntregue);
                //    break;
                //case "saldo":
                //    set = ascending ? set.OrderBy(l => l.Saldo) : set.OrderByDescending(l => l.Saldo);
                //    break;
                default:
                    set = ascending ? set.OrderBy(l => l.Id) : set.OrderByDescending(l => l.Id);
                    break;
            }

            return set.Skip(pageCount * pageIndex).Take(pageCount);
        }


        #endregion
    }
}