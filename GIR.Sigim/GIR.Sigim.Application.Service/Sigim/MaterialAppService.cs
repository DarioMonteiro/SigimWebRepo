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
    public class MaterialAppService : BaseAppService, IMaterialAppService
    {
        private IMaterialRepository MaterialRepository;

        public MaterialAppService(IMaterialRepository MaterialRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.MaterialRepository = MaterialRepository;
        }

        #region IMaterialAppService Members

        public List<MaterialDTO> ListarAtivosPeloTipoTabelaPropria(string descricao)
        {
            return MaterialRepository.ListarAtivosPeloTipoTabelaPropria(descricao, l => l.UnidadeMedida).To<List<MaterialDTO>>();
        }

        #endregion
    }
}