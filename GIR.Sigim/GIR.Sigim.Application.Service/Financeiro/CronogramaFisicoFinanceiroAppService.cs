using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using System.Threading.Tasks;
using GIR.Sigim.Application.Service.Financeiro;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class CronogramaFisicoFinanceiroAppService : BaseAppService, ICronogramaFisicoFinanceiroAppService
    {
        #region Declaração
        #endregion

        #region Construtor

        public CronogramaFisicoFinanceiroAppService(MessageQueue messageQueue)
            : base(messageQueue)
        {

        }

        #endregion

        #region Métodos ICronogramaFisicoFinanceiroAppService
        #endregion


    }
}
