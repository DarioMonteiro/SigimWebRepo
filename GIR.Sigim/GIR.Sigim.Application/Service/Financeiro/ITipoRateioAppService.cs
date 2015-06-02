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
    public interface ITipoRateioAppService
    {
        List<TipoRateioDTO> ListarTodos();
        List<TipoRateioDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros);
        TipoRateioDTO ObterPeloId(int? id);
        bool Salvar(TipoRateioDTO dto);
        bool Deletar(int? id);
    }
}
