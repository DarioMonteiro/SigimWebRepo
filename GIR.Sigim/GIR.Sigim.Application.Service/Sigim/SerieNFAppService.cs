using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Adapter;

namespace GIR.Sigim.Application.Service.Sigim
{
    public class SerieNFAppService : BaseAppService, ISerieNFAppService
    {
        private ISerieNFRepository serieNFRepository;

        public SerieNFAppService(ISerieNFRepository serieNFRepository, MessageQueue messageQueue)
            :base(messageQueue)
        {
            this.serieNFRepository = serieNFRepository;
        }

        public List<SerieNFDTO> ListarTodos()
        {
            return serieNFRepository.ListarTodos().To<List<SerieNFDTO>>();
        }
    }
}
