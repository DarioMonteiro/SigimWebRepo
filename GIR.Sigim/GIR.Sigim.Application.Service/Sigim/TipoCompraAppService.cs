using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Application.Service.Sigim
{
    public class TipoCompraAppService : BaseAppService, ITipoCompraAppService
    {
        private ITipoCompraRepository tipoCompraRepository;

        public TipoCompraAppService(ITipoCompraRepository tipoCompraRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tipoCompraRepository = tipoCompraRepository;
        }

        public List<TipoCompraDTO> ListarTodos()
        {
            return tipoCompraRepository.ListarTodos().To<List<TipoCompraDTO>>();
        }
    }
}
