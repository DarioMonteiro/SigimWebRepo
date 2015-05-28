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
    public class NaturezaOperacaoAppService : BaseAppService, INaturezaOperacaoAppService 
    {
        private INaturezaOperacaoRepository naturezaOperacaoRepository;

        public NaturezaOperacaoAppService(INaturezaOperacaoRepository naturezaOperacaoRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.naturezaOperacaoRepository = naturezaOperacaoRepository;
        }

        public List<NaturezaOperacaoDTO> ListarTodos()
        {
            return naturezaOperacaoRepository.ListarTodos().To<List<NaturezaOperacaoDTO>>();
        }
    }
}
