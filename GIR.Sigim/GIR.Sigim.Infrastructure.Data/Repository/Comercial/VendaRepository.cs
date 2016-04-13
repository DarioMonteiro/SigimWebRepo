using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Comercial;
using GIR.Sigim.Domain.Repository.Comercial;
using GIR.Sigim.Domain.Specification;
using System.Linq.Expressions;

namespace GIR.Sigim.Infrastructure.Data.Repository.Comercial
{
    public class VendaRepository : Repository<Venda>, IVendaRepository
    {
        #region Constructor

        public VendaRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IVendaRepository Members

        public override IEnumerable<Venda> ListarPeloFiltroComPaginacao(
            ISpecification<Venda> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<Venda, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            set = set.Where(specification.SatisfiedBy());

            totalRecords = set.Count();

            switch (orderBy)
            {
                case "incorporador":
                    set = ascending ? set.OrderBy(l => l.Contrato.Unidade.Empreendimento.Incorporador.RazaoSocial) : set.OrderByDescending(l => l.Contrato.Unidade.Empreendimento.Incorporador.RazaoSocial);
                    break;
                case "empreendimento":
                    set = ascending ? set.OrderBy(l => l.Contrato.Unidade.Empreendimento.Nome) : set.OrderByDescending(l => l.Contrato.Unidade.Empreendimento.Nome);
                    break;
                case "bloco":
                    set = ascending ? set.OrderBy(l => l.Contrato.Unidade.Bloco.Nome) : set.OrderByDescending(l => l.Contrato.Unidade.Bloco.Nome);
                    break;
                case "unidade":
                    set = ascending ? set.OrderBy(l => l.Contrato.Unidade.Descricao) : set.OrderByDescending(l => l.Contrato.Unidade.Descricao);
                    break;
                case "contratoId":
                    set = ascending ? set.OrderBy(l => l.Id) : set.OrderByDescending(l => l.Id);
                    break;
                case "descricaoSituacaoContrato":
                    set = ascending ? set.OrderBy(l => l.Contrato.SituacaoContrato) : set.OrderByDescending(l => l.Contrato.SituacaoContrato);
                    break;
                case "cliente":
                    set = ascending ? set.OrderBy(l => l.Contrato.ListaVendaParticipante.Where(p => p.TipoParticipanteId == GIR.Sigim.Domain.Constantes.Comercial.ContratoTipoParticipanteTitular).FirstOrDefault().Cliente.Nome) : set.OrderByDescending(l => l.Contrato.ListaVendaParticipante.Where(p => p.TipoParticipanteId == GIR.Sigim.Domain.Constantes.Comercial.ContratoTipoParticipanteTitular).FirstOrDefault().Cliente.Nome);
                    break;
                case "tabelaVenda":
                    set = ascending ? set.OrderBy(l => l.TabelaVenda.Nome) : set.OrderByDescending(l => l.TabelaVenda.Nome);
                    break;
                case "dataVenda":
                    set = ascending ? set.OrderBy(l => l.DataVenda) : set.OrderByDescending(l => l.DataVenda);
                    break;
                case "precoTabela":
                    set = ascending ? set.OrderBy(l => l.PrecoTabela) : set.OrderByDescending(l => l.PrecoTabela);
                    break;
                case "valorDesconto":
                    set = ascending ? set.OrderBy(l => l.ValorDesconto) : set.OrderByDescending(l => l.ValorDesconto);
                    break;
                case "percentualDesconto":
                    set = ascending ? set.OrderBy(l => l.ValorDesconto) : set.OrderByDescending(l => l.ValorDesconto);
                    break;
                case "precoPraticado":
                    set = ascending ? set.OrderBy(l => l.PrecoPraticado) : set.OrderByDescending(l => l.PrecoPraticado);
                    break;
                case "dataAssinatura":
                    set = ascending ? set.OrderBy(l => l.DataAssinatura) : set.OrderByDescending(l => l.DataAssinatura);
                    break;
                case "dataCancelamento":
                    set = ascending ? set.OrderBy(l => l.DataCancelamento) : set.OrderByDescending(l => l.DataCancelamento);
                    break;
                case "aprovado":
                    set = ascending ? set.OrderBy(l => l.Aprovado) : set.OrderByDescending(l => l.Aprovado);
                    break;
                case "usuarioAprovacao":
                    set = ascending ? set.OrderBy(l => l.UsuarioAprovacao) : set.OrderByDescending(l => l.UsuarioAprovacao);
                    break;
                case "dataAprovacao":
                    set = ascending ? set.OrderBy(l => l.DataAprovacao) : set.OrderByDescending(l => l.DataAprovacao);
                    break;
                default:
                    set = ascending ? set.OrderBy(l => l.Contrato.Unidade.Empreendimento.Incorporador.RazaoSocial) : set.OrderByDescending(l => l.Contrato.Unidade.Empreendimento.Incorporador.RazaoSocial);
                    break;
            }

            return set.Skip(pageCount * pageIndex).Take(pageCount);
        }

        #endregion
    }
}