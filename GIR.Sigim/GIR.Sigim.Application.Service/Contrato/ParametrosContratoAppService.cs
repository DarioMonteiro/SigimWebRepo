using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Contrato;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Repository.Contrato;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Application.Service.Contrato
{
    public class ParametrosContratoAppService : BaseAppService, IParametrosContratoAppService
    {
        private IParametrosContratoRepository parametrosRepository;

        public ParametrosContratoAppService(IParametrosContratoRepository parametrosRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.parametrosRepository = parametrosRepository;
        }

        public ParametrosContratoDTO Obter()
        {
            return parametrosRepository.ListarTodos().FirstOrDefault().To<ParametrosContratoDTO>();
        }

        public void AtualizarMascaraClasseInsumo(string mascaraClasseInsumo)
        {
            var parametros = parametrosRepository.ListarTodos().FirstOrDefault() ?? new ParametrosContrato();
            parametros.MascaraClasseInsumo = mascaraClasseInsumo;

            if (parametros.Id.HasValue)
                parametrosRepository.Alterar(parametros);
            else
                parametrosRepository.Inserir(parametros);
        }
    }
}