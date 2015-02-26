using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Orcamento;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Repository.Orcamento;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Application.Service.Orcamento
{
    public class ParametrosOrcamentoAppService : BaseAppService, IParametrosOrcamentoAppService
    {
        private IParametrosOrcamentoRepository parametrosRepository;

        public ParametrosOrcamentoAppService(IParametrosOrcamentoRepository parametrosRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.parametrosRepository = parametrosRepository;
        }

        public ParametrosOrcamentoDTO Obter()
        {
            return parametrosRepository.ListarTodos().FirstOrDefault().To<ParametrosOrcamentoDTO>();
        }

        public void AtualizarMascaraClasseInsumo(string mascaraClasseInsumo)
        {
            var parametros = parametrosRepository.ListarTodos().FirstOrDefault() ?? new ParametrosOrcamento();
            parametros.MascaraClasseInsumo = mascaraClasseInsumo;

            if (parametros.Id.HasValue)
                parametrosRepository.Alterar(parametros);
            else
                parametrosRepository.Inserir(parametros);
        }
    }
}