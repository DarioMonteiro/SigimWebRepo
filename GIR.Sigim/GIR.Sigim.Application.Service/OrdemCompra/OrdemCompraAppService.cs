using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Domain.Repository.OrdemCompra;
using GIR.Sigim.Application.Filtros.OrdemCompras;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Enums;
using GIR.Sigim.Domain.Specification.OrdemCompra;

namespace GIR.Sigim.Application.Service.OrdemCompra
{
    public class OrdemCompraAppService : BaseAppService, IOrdemCompraAppService
    {
        #region Declaração

        private IOrdemCompraRepository ordemCompraRepository;
        
        #endregion

        #region Construtor

        public OrdemCompraAppService(IOrdemCompraRepository ordemCompraRepository, 
                                     MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.ordemCompraRepository = ordemCompraRepository;
        }

        #endregion


        #region Métodos IOrdemCompraAppService

        public OrdemCompraDTO ObterPeloId(int? id)
        {
            return ordemCompraRepository.ObterPeloId(id, l => l.ListaItens).To<OrdemCompraDTO>();
        }

        public List<OrdemCompraDTO> PesquisarOrdensCompraPeloFiltro(OrdemCompraPesquisaFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<Domain.Entity.OrdemCompra.OrdemCompra>)new TrueSpecification<Domain.Entity.OrdemCompra.OrdemCompra>();
            int? inicio;
            int? fim;

            bool EhTipoSelecaoContem = filtro.TipoSelecao == TipoPesquisa.Contem;
            switch (filtro.Campo)
            {
                case "centroCusto":
                    specification &= EhTipoSelecaoContem ? OrdemCompraSpecification.CentroCustoContem(filtro.TextoInicio)
                        : OrdemCompraSpecification.CentroCustoNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "dataOrdemCompra":
                    Nullable<DateTime> datInicio = !string.IsNullOrEmpty(filtro.TextoInicio) ? Convert.ToDateTime(filtro.TextoInicio) : (Nullable<DateTime>)null;
                    Nullable<DateTime> datFim = !string.IsNullOrEmpty(filtro.TextoFim) ? Convert.ToDateTime(filtro.TextoFim) : (Nullable<DateTime>)null;
                    specification &= EhTipoSelecaoContem ? OrdemCompraSpecification.DataOrdemCompraContem(datInicio)
                        : OrdemCompraSpecification.DataOrdemCompraNoIntervalo(datInicio, datFim);
                    break;
                case "fornecedor":
                    specification &= EhTipoSelecaoContem ? OrdemCompraSpecification.FornecedorContem(filtro.TextoInicio)
                        : OrdemCompraSpecification.FornecedorNoIntervalo(filtro.TextoInicio, filtro.TextoFim);
                    break;
                case "id":
                default:
                    inicio = !string.IsNullOrEmpty(filtro.TextoInicio) ? Convert.ToInt32(filtro.TextoInicio) : (int?)null;
                    fim = !string.IsNullOrEmpty(filtro.TextoFim) ? Convert.ToInt32(filtro.TextoFim) : (int?)null;
                    specification &= EhTipoSelecaoContem ? OrdemCompraSpecification.MatchingId(inicio)
                        : OrdemCompraSpecification.IdNoIntervalo(inicio, fim);
                    break;
            }

            return ordemCompraRepository.Pesquisar(specification,
                                                   filtro.PageIndex,
                                                   filtro.PageSize,
                                                   filtro.OrderBy,
                                                   filtro.Ascending,
                                                   out totalRegistros,
                                                   l => l.CentroCusto,
                                                   l => l.ClienteFornecedor).To<List<OrdemCompraDTO>>();
        }

        #endregion
    }
}
