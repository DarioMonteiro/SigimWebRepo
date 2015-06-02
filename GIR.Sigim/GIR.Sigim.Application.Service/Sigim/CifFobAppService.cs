using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Service.Sigim
{
    public class CifFobAppService : BaseAppService, ICifFobAppService
    {
        private ICifFobRepository cifFobRepository;

        public CifFobAppService(ICifFobRepository cifFobRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.cifFobRepository = cifFobRepository;
        }

        public List<CifFobDTO> ListarTodos()
        {
            return cifFobRepository.ListarTodos().To<List<CifFobDTO>>();
        }

    }
}
