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
    public class NaturezaReceitaAppService : BaseAppService, INaturezaReceitaAppService 
    {
        private INaturezaReceitaRepository naturezaReceitaRepository;

        public NaturezaReceitaAppService(INaturezaReceitaRepository naturezaReceitaRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.naturezaReceitaRepository = naturezaReceitaRepository;
        }

        public List<NaturezaReceitaDTO> ListarTodos()
        {
            return naturezaReceitaRepository.ListarTodos().To<List<NaturezaReceitaDTO>>();
        }
    }
}