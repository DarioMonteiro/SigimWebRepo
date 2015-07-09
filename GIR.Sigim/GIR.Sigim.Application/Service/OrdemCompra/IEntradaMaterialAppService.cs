using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.Filtros.OrdemCompras;

namespace GIR.Sigim.Application.Service.OrdemCompra
{
    public interface IEntradaMaterialAppService : IBaseAppService
    {
        List<EntradaMaterialDTO> ListarPeloFiltro(EntradaMaterialFiltro filtro, out int totalRegistros);
    }
}