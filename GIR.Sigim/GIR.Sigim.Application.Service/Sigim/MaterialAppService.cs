using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.Sigim;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Application.Service.Sigim
{
    public class MaterialAppService : BaseAppService, IMaterialAppService
    {
        private IMaterialRepository materialRepository;

        public MaterialAppService(IMaterialRepository MaterialRepository, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.materialRepository = MaterialRepository;
        }

        #region IMaterialAppService Members

        public List<MaterialDTO> ListarPeloFiltro(MaterialFiltro filtro, out int totalRegistros)
        {
            return materialRepository.ListarPeloFiltroComPaginacao(
                l => l.Descricao.Contains(filtro.Descricao),
                filtro.PaginationParameters.PageIndex,
                filtro.PaginationParameters.PageSize,
                filtro.PaginationParameters.OrderBy,
                filtro.PaginationParameters.Ascending,
                out totalRegistros).To<List<MaterialDTO>>();
        }

        public List<MaterialDTO> ListarAtivosPeloTipoTabelaPropria(string descricao)
        {
            return materialRepository.ListarAtivosPeloTipoTabelaPropria(descricao, l => l.UnidadeMedida).To<List<MaterialDTO>>();
        }

        #endregion
    }
}