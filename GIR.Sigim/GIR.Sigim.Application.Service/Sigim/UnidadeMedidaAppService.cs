using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Application.Service.Sigim
{
    public class UnidadeMedidaAppService : BaseAppService, IUnidadeMedidaAppService
    {
        private IUnidadeMedidaRepository unidadeMedidaRepository;

        public UnidadeMedidaAppService(IUnidadeMedidaRepository unidadeMedidaRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.unidadeMedidaRepository = unidadeMedidaRepository;
        }

        #region IUnidadeMedidaAppService Members

        public List<UnidadeMedidaDTO> ListarTodos()
        {
            return unidadeMedidaRepository.ListarTodos().To<List<UnidadeMedidaDTO>>();
        }

        #endregion
    }
}