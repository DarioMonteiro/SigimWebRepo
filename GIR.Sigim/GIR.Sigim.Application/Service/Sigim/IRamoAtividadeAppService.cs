using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface IRamoAtividadeAppService
    {
        List<RamoAtividadeDTO> ListarTodos();
        List<RamoAtividadeDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros);
        RamoAtividadeDTO ObterPeloId(int? id);
        bool Salvar(RamoAtividadeDTO dto);
        bool Deletar(int? id);
    }
}