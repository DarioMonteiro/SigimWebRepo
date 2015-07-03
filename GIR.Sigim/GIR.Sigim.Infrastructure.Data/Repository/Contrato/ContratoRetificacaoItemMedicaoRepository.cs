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
    public class ContratoRetificacaoItemMedicaoRepository : Repository<ContratoRetificacaoItemMedicao>, IContratoRetificacaoItemMedicaoRepository
    {
        #region Construtor

        public ContratoRetificacaoItemMedicaoRepository(UnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }
        #endregion

        #region IRepository<ContratoRetificacaoItemMedicao> Members

        public override IEnumerable<ContratoRetificacaoItemMedicao> ListarPeloFiltroComPaginacao(
            ISpecification<ContratoRetificacaoItemMedicao> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<ContratoRetificacaoItemMedicao, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            set = set.Where(specification.SatisfiedBy());

            totalRecords = set.Count();

            switch (orderBy)
            {
                //<th class="sorting @fornecedorClienteSortingSuffix text-left" data-order="fornecedorCliente" style="min-width:140px;">Fornecedor / Cliente</th>
                //<th class="sorting @CPFCNPJFornecedorClienteSortingSuffix text-left" data-order="CPFCNPJFornecedorCliente" style="min-width:90px;">CPF / CNPJ</th>

                //<th class="sorting @valorClasseSortingSuffix text-left" data-order="valorClasse">Valor classe</th>
                //<th class="sorting @tituloSortingSuffix text-left" data-order="titulo" style="min-width:60px;">Título</th>

                case "fornecedorCliente":
                    set = ascending ? set.OrderBy(l => l.Contrato.Contratado.Nome).ThenBy(l => l.MultiFornecedor.Nome).ThenBy(l => l.Contrato.Contratante.Nome) :
                                      set.OrderByDescending(l => l.Contrato.Contratado.Nome).ThenByDescending(l => l.MultiFornecedor.Nome).ThenByDescending(l => l.Contrato.Contratante.Nome);
                    break;
                case "titulo":
                    set = ascending ? set.OrderBy(l => l.TituloReceberId).ThenBy(l => l.TituloPagarId) : set.OrderByDescending(l => l.TituloReceberId).ThenByDescending(l => l.TituloPagarId);
                    break;
                case "numeroDocumento":
                    set = ascending ? set.OrderBy(l => l.NumeroDocumento) : set.OrderByDescending(l => l.NumeroDocumento);
                    break;
                case "dataEmissao":
                    set = ascending ? set.OrderBy(l => l.DataEmissao) : set.OrderByDescending(l => l.DataEmissao);
                    break;
                case "dataVencimento":
                    set = ascending ? set.OrderBy(l => l.DataVencimento) : set.OrderByDescending(l => l.DataVencimento);
                    break;
                case "valor":
                    set = ascending ? set.OrderBy(l => l.Valor) : set.OrderByDescending(l => l.Valor);
                    break;
                case "descricaoItem":
                    set = ascending ? set.OrderBy(l => l.ContratoRetificacaoItem.Servico.Descricao) : set.OrderByDescending(l => l.ContratoRetificacaoItem.Servico.Descricao);
                    break;
                case "codigoDescricaoClasse":
                    set = ascending ? set.OrderBy(l => l.ContratoRetificacaoItem.Classe.Codigo) : set.OrderByDescending(l => l.ContratoRetificacaoItem.Classe.Codigo);
                    break;
                case "contrato":
                    set = ascending ? set.OrderBy(l => l.ContratoId) : set.OrderByDescending(l => l.ContratoId);
                    break;
                case "descricaoContrato":
                    set = ascending ? set.OrderBy(l => l.Contrato.ContratoDescricao.Descricao) : set.OrderByDescending(l => l.Contrato.ContratoDescricao.Descricao);
                    break;
                case "usuarioLiberacao":
                    set = ascending ? set.OrderBy(l => l.UsuarioLiberacao) : set.OrderByDescending(l => l.UsuarioLiberacao);
                    break;
                case "codigoDescricaoCentroCusto":
                    set = ascending ? set.OrderBy(l => l.Contrato.CentroCusto.Codigo) : set.OrderByDescending(l => l.Contrato.CentroCusto.Codigo);
                    break;
                case "id":
                default:
                    set = ascending ? set.OrderBy(l => l.Id) : set.OrderByDescending(l => l.Id);
                    break;
            }

            return set.Skip(pageCount * pageIndex).Take(pageCount);
        }

        #endregion
    }
}
