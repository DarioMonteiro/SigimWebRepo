using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Repository.Contrato;
using GIR.Sigim.Application.DTO.Contrato;
using GIR.Sigim.Domain.Specification.Contrato;  

namespace GIR.Sigim.Application.Service.Contrato
{
    public class ContratoRetificacaoItemAppService : BaseAppService , IContratoRetificacaoItemAppService
    {
        #region Declaração

        private IContratoRetificacaoItemRepository contratoRetificacaoItemRepository;

        #endregion

        #region Constructor

        public ContratoRetificacaoItemAppService(IContratoRetificacaoItemRepository contratoRetificacaoItemRepository,MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.contratoRetificacaoItemRepository = contratoRetificacaoItemRepository;
        }
        #endregion

        #region Métodos IContratoRetificacaoItemAppService

        public ContratoRetificacaoItemDTO ObterPeloId(int? id)
        {
            return contratoRetificacaoItemRepository.ObterPeloId(id,l => l.Classe, l => l.Servico.UnidadeMedida , l => l.RetencaoTipoCompromisso).To<ContratoRetificacaoItemDTO>() ;
        }

        #endregion
    }
}
