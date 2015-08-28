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
    public class ComplementoNaturezaOperacaoAppService : BaseAppService, IComplementoNaturezaOperacaoAppService 
    {
        private IComplementoNaturezaOperacaoRepository complementoNaturezaOperacaoRepository;

        public ComplementoNaturezaOperacaoAppService(IComplementoNaturezaOperacaoRepository complementoNaturezaOperacaoRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.complementoNaturezaOperacaoRepository = complementoNaturezaOperacaoRepository;
        }

        public List<ComplementoNaturezaOperacaoDTO> ListarTodos()
        {
            return complementoNaturezaOperacaoRepository.ListarTodos().To<List<ComplementoNaturezaOperacaoDTO>>();
        }
    }
}