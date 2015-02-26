using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Application.Service.Sigim
{
    public class AssuntoContatoAppService : BaseAppService, IAssuntoContatoAppService
    {
        private IAssuntoContatoRepository assuntoContatoRepository;

        public AssuntoContatoAppService(IAssuntoContatoRepository AssuntoContatoRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.assuntoContatoRepository = AssuntoContatoRepository;
        }

        public List<AssuntoContatoDTO> ListarTodos()
        {
            return assuntoContatoRepository.ListarTodos().To<List<AssuntoContatoDTO>>();
        }
    }
}