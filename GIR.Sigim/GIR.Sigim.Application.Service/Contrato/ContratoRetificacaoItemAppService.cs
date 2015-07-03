using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Repository.Contrato;
using GIR.Sigim.Application.DTO.Contrato;

namespace GIR.Sigim.Application.Service.Contrato
{
    public class ContratoRetificacaoItemAppService : BaseAppService , IContratoRetificacaoItemAppService
    {
        #region Declaração

        #endregion

        #region Constructor

        public ContratoRetificacaoItemAppService(MessageQueue messageQueue)
            : base(messageQueue)
        {
        }
        #endregion

        #region Métodos IContratoRetificacaoItemAppService

        public bool EhNaturezaItemPrecoGlobal(ContratoRetificacaoItemDTO dto)
        {
            if (dto.NaturezaItem != Domain.Entity.Contrato.NaturezaItem.PrecoGlobal)
            {
                return false;
            }

            return true;
        }

        public bool EhNaturezaItemPrecoUnitario(ContratoRetificacaoItemDTO dto)
        {
            if (dto.NaturezaItem != Domain.Entity.Contrato.NaturezaItem.PrecoUnitario)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
