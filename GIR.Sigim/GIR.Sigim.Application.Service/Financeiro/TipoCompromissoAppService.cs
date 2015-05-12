using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Resource;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class TipoCompromissoAppService : BaseAppService, ITipoCompromissoAppService
    {
        private ITipoCompromissoRepository tipoCompromissoRepository;

        public TipoCompromissoAppService(ITipoCompromissoRepository tipoCompromissoRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tipoCompromissoRepository = tipoCompromissoRepository;
        }

        #region ITipoCompromissoAppService Members

        public List<TipoCompromissoDTO> ListarTipoPagar()
        {
            return tipoCompromissoRepository.ListarPeloFiltro(l => l.TipoPagar.HasValue && l.TipoPagar.Value).To<List<TipoCompromissoDTO>>();
        }

        #endregion
    }
}