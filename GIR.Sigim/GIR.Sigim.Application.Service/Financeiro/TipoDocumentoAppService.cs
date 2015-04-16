using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class TipoDocumentoAppService : BaseAppService, ITipoDocumentoAppService
    {
        private ITipoDocumentoRepository tipoDocumentoRepository;

        public TipoDocumentoAppService(ITipoDocumentoRepository tipoDocumentoRepository, MessageQueue messageQueue) 
            : base (messageQueue)
        {
            this.tipoDocumentoRepository = tipoDocumentoRepository;
        }

        #region métodos de ITipoDocumentoAppService

        public List<TipoDocumentoDTO> ListarTodos()
        {
            return tipoDocumentoRepository.ListarTodos().OrderBy(l => l.Sigla).To<List<TipoDocumentoDTO>>(); 
        }

        #endregion
    }
}
