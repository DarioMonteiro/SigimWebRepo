using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface IGrupoAppService
    {
        List<GrupoDTO> ListarTodos();
        List<GrupoDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros);
        GrupoDTO ObterPeloId(int? id);
        bool Salvar(GrupoDTO dto);
        bool Deletar(int? id);
    }
}