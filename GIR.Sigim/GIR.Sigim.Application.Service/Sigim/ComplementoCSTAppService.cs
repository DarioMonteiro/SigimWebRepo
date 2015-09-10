using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Application.Service.Sigim
{
    public class ComplementoCSTAppService : BaseAppService, IComplementoCSTAppService 
    {
        private IComplementoCSTRepository complementoCSTRepository;

        public ComplementoCSTAppService(IComplementoCSTRepository complementoCSTRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.complementoCSTRepository = complementoCSTRepository;
        }

        public List<ComplementoCSTDTO> ListarTodos()
        {
            return complementoCSTRepository.ListarTodos().To<List<ComplementoCSTDTO>>();
        }
    }
}