using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;

namespace GIR.Sigim.Application.Service.Sigim
{
    public class CSTAppService : BaseAppService , ICSTAppService
    {
        private ICSTRepository cstRepository;

        public CSTAppService(ICSTRepository cstRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.cstRepository = cstRepository;
        }

        public List<CSTDTO> ListarTodos()
        {
            return cstRepository.ListarTodos().To<List<CSTDTO>>();
        }
    }
}
