using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Infrastructure.Crosscutting.Security;
using GIR.Sigim.Application.DTO.Comercial;
using GIR.Sigim.Domain.Entity.Comercial;
using GIR.Sigim.Domain.Repository.Comercial;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Domain.Specification.Comercial;
using GIR.Sigim.Application.Filtros.Comercial;
using GIR.Sigim.Application.Constantes;

namespace GIR.Sigim.Application.Service.Comercial
{
    public class VendaAppService : BaseAppService, IVendaAppService
    {
        private IVendaRepository vendaRepository;


        public VendaAppService(IVendaRepository vendaRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.vendaRepository = vendaRepository;
        }

        #region IVendaRepositoryAppService Members

        public List<RelStatusVendaDTO> ListarPeloFiltroRelStatusVenda(RelStatusVendaFiltro filtro, out int totalRegistros)
        {
            var specification = (Specification<Venda>)new TrueSpecification<Venda>();

            specification &= VendaSpecification.IgualAoIncorporadorId(filtro.IncorporadorId );
            specification &= VendaSpecification.IgualAoEmpreendimentoId(filtro.EmpreendimentoId );
            specification &= VendaSpecification.IgualAoBlocoId(filtro.BlocoId );


            if (filtro.SituacaoTodas)
            {
                specification &= (
                                 (VendaSpecification.EhProposta()) ||
                                 (VendaSpecification.EhAssinada()) ||
                                 (VendaSpecification.EhCancelada()) ||
                                 (VendaSpecification.EhRescindida()) ||
                                 (VendaSpecification.EhQuitada()) ||
                                 (VendaSpecification.EhEscriturada())
                                 );
            }
            else
            {
                specification &= (
                                 (filtro.SituacaoProposta ? VendaSpecification.EhProposta() : new FalseSpecification<Venda>()) ||
                                 (filtro.SituacaoAssinada ? VendaSpecification.EhAssinada() : new FalseSpecification<Venda>()) ||
                                 (filtro.SituacaoCancelada ? VendaSpecification.EhCancelada() : new FalseSpecification<Venda>()) ||
                                 (filtro.SituacaoCancelada ? VendaSpecification.EhRescindida() : new FalseSpecification<Venda>()) ||
                                 (filtro.SituacaoCancelada ? VendaSpecification.EhQuitada() : new FalseSpecification<Venda>()) ||
                                 (filtro.SituacaoCancelada ? VendaSpecification.EhEscriturada() : new FalseSpecification<Venda>())
                                 );
            }

            
            return vendaRepository.ListarPeloFiltroComPaginacao(
                specification,
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<RelStatusVendaDTO>>();
        }

        public bool EhPermitidoImprimirRelStatusVenda()
        {
            if (!UsuarioLogado.IsInRole(Funcionalidade.RelStatusVendaImprimir))
                return false;

            return true;
        }
        
        #endregion

        #region Métodos Privados

        #endregion
    }
}