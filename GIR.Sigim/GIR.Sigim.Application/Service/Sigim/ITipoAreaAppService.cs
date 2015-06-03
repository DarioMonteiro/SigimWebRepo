using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface ITipoAreaAppService
    {
        List<TipoAreaDTO> ListarTodos();
        List<TipoAreaDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros);
        TipoAreaDTO ObterPeloId(int? id);
        bool Salvar(TipoAreaDTO dto);
        bool Deletar(int? id);
    }
}