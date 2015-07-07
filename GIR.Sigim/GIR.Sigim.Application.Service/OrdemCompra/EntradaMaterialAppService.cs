using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CrystalDecisions.Shared;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.OrdemCompras;
using GIR.Sigim.Application.Reports.OrdemCompra;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Repository.OrdemCompra;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Domain.Specification.OrdemCompra;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Application.Service.OrdemCompra
{
    public class EntradaMaterialAppService : BaseAppService, IEntradaMaterialAppService
    {
        private IEntradaMaterialRepository entradaMaterialRepository;
        private IUsuarioAppService usuarioAppService;

        public EntradaMaterialAppService(
            IEntradaMaterialRepository entradaMaterialRepository,
            IUsuarioAppService usuarioAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.entradaMaterialRepository = entradaMaterialRepository;
            this.usuarioAppService = usuarioAppService;
        }

        #region IEntradaMaterialAppService Members

        public List<EntradaMaterialDTO> ListarPeloFiltro(EntradaMaterialFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<EntradaMaterial>)new TrueSpecification<EntradaMaterial>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(UsuarioLogado.Id, Resource.Sigim.NomeModulo.OrdemCompra))
                specification &= EntradaMaterialSpecification.UsuarioPossuiAcessoAoCentroCusto(UsuarioLogado.Id, Resource.Sigim.NomeModulo.OrdemCompra);

            if (filtro.Id.HasValue)
                specification &= EntradaMaterialSpecification.MatchingId(filtro.Id);
            else
            {
                specification &= EntradaMaterialSpecification.DataMaiorOuIgual(filtro.DataInicial);
                specification &= EntradaMaterialSpecification.DataMenorOuIgual(filtro.DataFinal);
                specification &= EntradaMaterialSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);

                if (filtro.EhPendente || filtro.EhCancelada || filtro.EhFechada)
                {
                    specification &= ((filtro.EhPendente ? EntradaMaterialSpecification.EhPendente() : new FalseSpecification<EntradaMaterial>())
                        || ((filtro.EhCancelada) ? EntradaMaterialSpecification.EhCancelada() : new FalseSpecification<EntradaMaterial>())
                        || ((filtro.EhFechada) ? EntradaMaterialSpecification.EhFechada() : new FalseSpecification<EntradaMaterial>()));
                }
            }

            return entradaMaterialRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros,
                l => l.CentroCusto,
                l => l.ClienteFornecedor,
                l => l.FornecedorNota).To<List<EntradaMaterialDTO>>();
        }


        #endregion

        #region Métodos Privados

        #endregion
    }
}