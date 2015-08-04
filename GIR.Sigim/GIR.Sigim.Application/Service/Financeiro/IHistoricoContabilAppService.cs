using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public interface IHistoricoContabilAppService
    {
        List<HistoricoContabilDTO> ListarTodos();
        List<HistoricoContabilDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros);
        HistoricoContabilDTO ObterPeloId(int? id);
        bool Salvar(HistoricoContabilDTO dto);
        bool Deletar(int? id);
    }
}
