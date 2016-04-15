using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Adapter;

namespace GIR.Sigim.Application.Service.Sigim
{
    public class IndiceFinanceiroAppService : BaseAppService, IIndiceFinanceiroAppService
    {
        private IIndiceFinanceiroRepository indiceFinanceiroRepository;

        public IndiceFinanceiroAppService(IIndiceFinanceiroRepository indiceFinanceiroRepository, 
                                          MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.indiceFinanceiroRepository = indiceFinanceiroRepository;
        }

        public List<IndiceFinanceiroDTO> ListarTodos()
        {
            return indiceFinanceiroRepository.ListarTodos().To<List<IndiceFinanceiroDTO>>();
        }

    }
}
