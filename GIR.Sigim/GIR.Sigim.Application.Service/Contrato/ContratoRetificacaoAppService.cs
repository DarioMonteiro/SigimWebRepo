using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.DTO.Contrato;

namespace GIR.Sigim.Application.Service.Contrato
{
    public class ContratoRetificacaoAppService : BaseAppService, IContratoRetificacaoAppService
    {
        #region Declaracao
        #endregion

        #region Construtor

        public ContratoRetificacaoAppService(MessageQueue messageQueue)
            : base(messageQueue)
        {
        }

        #endregion

        #region Metodos IContratoRetificacaoAppService

        public bool EhRetificacaoExistente(ContratoRetificacaoDTO dto)
        {

            if (dto == null)
            {
                messageQueue.Add(Application.Resource.Contrato.ErrorMessages.RetificacaoNaoExiste, TypeMessage.Error);
                return false;
            }

            return true;
        }

        public bool EhRetificacaoAprovada(ContratoRetificacaoDTO dto)
        {

            if (!dto.Aprovada)
            {
                messageQueue.Add(Application.Resource.Contrato.ErrorMessages.RetificacaoNaoAprovada, TypeMessage.Error);
                return false;
            }

            return true;

        }

        #endregion


    }
}
