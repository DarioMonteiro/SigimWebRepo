using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Service.Orcamento;
using GIR.Sigim.Domain.Repository.Orcamento;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.DTO.Orcamento;


namespace GIR.Sigim.Application.Service.Orcamento
{
    public class ObraAppService : BaseAppService, IObraAppService
    {
        #region "Declaração"

        private IObraRepository ObraRepository;

        #endregion

        #region "Construtor"

        public ObraAppService(IObraRepository ObraRepository,   
                              MessageQueue messageQueue)
                : base(messageQueue)
            {
                this.ObraRepository = ObraRepository;
            }
        #endregion

        #region IObraAppService Members


        #endregion

    }
}
