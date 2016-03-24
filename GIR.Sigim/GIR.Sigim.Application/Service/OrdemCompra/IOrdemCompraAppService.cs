using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.Filtros.OrdemCompras;

namespace GIR.Sigim.Application.Service.OrdemCompra
{
    public interface IOrdemCompraAppService : IBaseAppService
    {
        OrdemCompraDTO ObterPeloId(int? id);
        List<OrdemCompraDTO> PesquisarOrdensCompraPeloFiltro(OrdemCompraPesquisaFiltro filtro, out int totalRegistros);
    }
}