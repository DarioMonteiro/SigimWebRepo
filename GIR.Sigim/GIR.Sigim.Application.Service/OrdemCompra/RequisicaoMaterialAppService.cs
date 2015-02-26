using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.Filtros.OrdemCompras;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Repository.OrdemCompra;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Domain.Specification.OrdemCompra;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Application.Service.OrdemCompra
{
    public class RequisicaoMaterialAppService : BaseAppService, IRequisicaoMaterialAppService
    {
        private IRequisicaoMaterialRepository requisicaoMaterialRepository;
        private IUsuarioAppService usuarioAppService;

        public RequisicaoMaterialAppService(
            IRequisicaoMaterialRepository requisicaoMaterialRepository,
            IUsuarioAppService usuarioAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.requisicaoMaterialRepository = requisicaoMaterialRepository;
            this.usuarioAppService = usuarioAppService;
        }

        #region IRequisicaoMaterialAppService Members

        public List<RequisicaoMaterialDTO> ListarPeloFiltro(RequisicaoMaterialFiltro filtro, int? idUsuario, out int totalRegistros)
        {
            var specification = (Specification<RequisicaoMaterial>)new TrueSpecification<RequisicaoMaterial>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.OrdemCompra))
                specification &= RequisicaoMaterialSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.OrdemCompra);

            if (filtro.Id.HasValue)
                specification &= RequisicaoMaterialSpecification.MatchingId(filtro.Id);
            else
            {
                specification &= RequisicaoMaterialSpecification.DataMaiorOuIgual(filtro.DataInicial);
                specification &= RequisicaoMaterialSpecification.DataMenorOuIgual(filtro.DataFinal);
                specification &= RequisicaoMaterialSpecification.PertenceAoCentroCusto(filtro.CentroCusto.Codigo);

                if (filtro.EhAprovada || filtro.EhCancelada || filtro.EhFechada || filtro.EhRequisitada)
                {
                    specification &= ((filtro.EhAprovada ? RequisicaoMaterialSpecification.EhAprovada() : new FalseSpecification<RequisicaoMaterial>())
                        || ((filtro.EhCancelada) ? RequisicaoMaterialSpecification.EhCancelada() : new FalseSpecification<RequisicaoMaterial>())
                        || ((filtro.EhFechada) ? RequisicaoMaterialSpecification.EhFechada() : new FalseSpecification<RequisicaoMaterial>())
                        || ((filtro.EhRequisitada) ? RequisicaoMaterialSpecification.EhRequisitada() : new FalseSpecification<RequisicaoMaterial>()));
                }
            }

            return requisicaoMaterialRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros,
                l => l.ListaItens.Select(c => c.RequisicaoMaterial)).To<List<RequisicaoMaterialDTO>>();
        }

        public RequisicaoMaterialDTO ObterPeloId(int? id)
        {
            return requisicaoMaterialRepository.ObterPeloId(id).To<RequisicaoMaterialDTO>();
        }

        #endregion
    }
}