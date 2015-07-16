using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.Filtros.OrdemCompras;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Domain.Repository.OrdemCompra;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Domain.Specification.OrdemCompra;

namespace GIR.Sigim.Application.Service.OrdemCompra
{
    public class OrdemCompraItemAppService: BaseAppService, IOrdemCompraItemAppService
    {

        #region Declaração
            private IUsuarioAppService usuarioAppService;
            private IOrdemCompraItemRepository ordemCompraItemRepository;
        #endregion

        #region Construtor

        public OrdemCompraItemAppService(IUsuarioAppService usuarioAppService,
                                         IOrdemCompraItemRepository ordemCompraItemRepository,
                                         MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.usuarioAppService = usuarioAppService;
            this.ordemCompraItemRepository = ordemCompraItemRepository;
        }

        #endregion

        #region Métodos IOrdemCompraItemAppService

        public List<RelOCItensOrdemCompraDTO> ListarPeloFiltroRelOCItensOrdemCompra(RelOcItensOrdemCompraFiltro filtro, int? idUsuario, out int totalRegistros)
        {
            var specification = MontarSpecificationRelOCItensOrdemCompra(filtro, idUsuario);

            var listaOrdemCompraItem =
             ordemCompraItemRepository.ListarPeloFiltroComPaginacao(specification,
                                                                    filtro.PaginationParameters.PageIndex,
                                                                    filtro.PaginationParameters.PageSize,
                                                                    filtro.PaginationParameters.OrderBy,
                                                                    filtro.PaginationParameters.Ascending,
                                                                    out totalRegistros,
                                                                    l => l.Classe,
                                                                    l => l.OrdemCompra.CentroCusto,
                                                                    l => l.OrdemCompra.ClienteFornecedor,
                                                                    l => l.Material.MaterialClasseInsumo).To<List<OrdemCompraItem>>();
            List<RelOCItensOrdemCompraDTO> listaRelOCItensOrdemCompra = new List<RelOCItensOrdemCompraDTO>();
            foreach (var item in listaOrdemCompraItem)
            {
                RelOCItensOrdemCompraDTO relat = new RelOCItensOrdemCompraDTO();

                relat.OrdemCompra = item.OrdemCompra.To<OrdemCompraDTO>();
                relat.OrdemCompraItem = item.To<OrdemCompraItemDTO>();

                listaRelOCItensOrdemCompra.Add(relat);
            }

            return listaRelOCItensOrdemCompra;

        }

        private Specification<OrdemCompraItem> MontarSpecificationRelOCItensOrdemCompra(RelOcItensOrdemCompraFiltro filtro, int? idUsuario)
        {
            var specification = (Specification<OrdemCompraItem>)new TrueSpecification<OrdemCompraItem>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.OrdemCompra))
                specification &= OrdemCompraItemSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.OrdemCompra);

            if (filtro.OrdemCompraId.HasValue)
                specification &= OrdemCompraItemSpecification.PertenceAhOrdemCompraId(filtro.OrdemCompraId);
            else
            {
                specification &= OrdemCompraItemSpecification.DataOrdemCompraMaiorOuIgual(filtro.DataInicial);
                specification &= OrdemCompraItemSpecification.DataOrdemCompraMenorOuIgual(filtro.DataFinal);
                specification &= OrdemCompraItemSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);
                specification &= OrdemCompraItemSpecification.PertenceAhClasseIniciadaPor(filtro.Classe.Codigo);
                specification &= OrdemCompraItemSpecification.PertenceAhClasseInsumoIniciadaPor(filtro.ClasseInsumo.Codigo);
                //specification &= OrdemCompraItemSpecification.ClienteFornecedorPertenceAhItemOC(filtro.ClienteFornecedor.Id);
                //specification &= OrdemCompraItemSpecification.PertenceMaterialId(filtro.Material.Id);
                specification &= OrdemCompraItemSpecification.EhExibirSomenteComSaldo(filtro.EhExibirSomentecomSaldo);

                if (filtro.EhFechada || filtro.EhLiberada || filtro.EhPendente)
                {
                    specification &= ((filtro.EhFechada ? OrdemCompraItemSpecification.EhFechada() : new FalseSpecification<OrdemCompraItem>())
                        || ((filtro.EhLiberada) ? OrdemCompraItemSpecification.EhLiberada() : new FalseSpecification<OrdemCompraItem>())
                        || ((filtro.EhPendente) ? OrdemCompraItemSpecification.EhPendente() : new FalseSpecification<OrdemCompraItem>()));
                }

            }

            return specification;
        }

        #endregion
    }

}
