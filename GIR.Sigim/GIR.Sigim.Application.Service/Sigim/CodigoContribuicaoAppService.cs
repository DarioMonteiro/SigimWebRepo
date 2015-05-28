using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Adapter;

namespace GIR.Sigim.Application.Service.Sigim
{
    public class CodigoContribuicaoAppService : BaseAppService, ICodigoContribuicaoAppService 
    {
        private ICodigoContribuicaoRepository codigoContribuicaoRepository;

        public CodigoContribuicaoAppService(ICodigoContribuicaoRepository codigoContribuicaoRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.codigoContribuicaoRepository = codigoContribuicaoRepository;
        }

        public List<CodigoContribuicaoDTO> ListarTodos()
        {
            return codigoContribuicaoRepository.ListarTodos().To<List<CodigoContribuicaoDTO>>();
        }
    }
}
