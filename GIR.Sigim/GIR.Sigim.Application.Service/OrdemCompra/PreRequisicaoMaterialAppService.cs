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
    public class PreRequisicaoMaterialAppService : BaseAppService, IPreRequisicaoMaterialAppService
    {
        private IPreRequisicaoMaterialRepository preRequisicaoMaterialRepository;
        private IUsuarioAppService usuarioAppService;

        public PreRequisicaoMaterialAppService(
            IPreRequisicaoMaterialRepository preRequisicaoMaterialRepository,
            IUsuarioAppService usuarioAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.preRequisicaoMaterialRepository = preRequisicaoMaterialRepository;
            this.usuarioAppService = usuarioAppService;
        }

        #region IPreRequisicaoMaterialAppService Members

        public List<PreRequisicaoMaterialDTO> ListarPeloFiltro(PreRequisicaoMaterialFiltro filtro, int? idUsuario, out int totalRegistros)
        {
            var specification = (Specification<PreRequisicaoMaterial>)new TrueSpecification<PreRequisicaoMaterial>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.OrdemCompra))
                specification &= PreRequisicaoMaterialSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.OrdemCompra);

            if (filtro.Id.HasValue)
                specification &= PreRequisicaoMaterialSpecification.MatchingId(filtro.Id);
            else
            {
                specification &= PreRequisicaoMaterialSpecification.DataMaiorOuIgual(filtro.DataInicial);
                specification &= PreRequisicaoMaterialSpecification.DataMenorOuIgual(filtro.DataFinal);

                if (filtro.EhCancelada || filtro.EhFechada || filtro.EhParcialmenteAprovada || filtro.EhRequisitada)
                {
                    specification &= ((filtro.EhCancelada ? PreRequisicaoMaterialSpecification.EhCancelada() : new FalseSpecification<PreRequisicaoMaterial>())
                        || ((filtro.EhFechada) ? PreRequisicaoMaterialSpecification.EhFechada() : new FalseSpecification<PreRequisicaoMaterial>())
                        || ((filtro.EhParcialmenteAprovada) ? PreRequisicaoMaterialSpecification.EhParcialmenteAprovada() : new FalseSpecification<PreRequisicaoMaterial>())
                        || ((filtro.EhRequisitada) ? PreRequisicaoMaterialSpecification.EhRequisitada() : new FalseSpecification<PreRequisicaoMaterial>()));
                }
            }

            return preRequisicaoMaterialRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros,
                l => l.ListaItens.Select(c => c.PreRequisicaoMaterial),
                l => l.ListaItens.Select(c => c.CentroCusto.ListaUsuarioCentroCusto)).To<List<PreRequisicaoMaterialDTO>>();
        }

        public PreRequisicaoMaterialDTO ObterPeloId(int? id, int? idUsuario)
        {
            var specification = (Specification<PreRequisicaoMaterial>)new TrueSpecification<PreRequisicaoMaterial>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.OrdemCompra))
                specification &= PreRequisicaoMaterialSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.OrdemCompra);

            return preRequisicaoMaterialRepository.ObterPeloId(id, specification, l => l.ListaItens.Select(s => s.ListaRequisicaoMaterialItem.Select(c => c.RequisicaoMaterial))).To<PreRequisicaoMaterialDTO>();
        }

        #endregion
    }
}