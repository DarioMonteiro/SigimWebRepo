using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface ITipologiaAppService
    {
        List<TipologiaDTO> ListarTodos();
        List<TipologiaDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros);
        TipologiaDTO ObterPeloId(int? id);
        bool Salvar(TipologiaDTO dto);
        bool Deletar(int? id);
    }
}