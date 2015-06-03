using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface ITipoCaracteristicaAppService
    {
        List<TipoCaracteristicaDTO> ListarTodos();
        List<TipoCaracteristicaDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros);
        TipoCaracteristicaDTO ObterPeloId(int? id);
        bool Salvar(TipoCaracteristicaDTO dto);
        bool Deletar(int? id);
    }
}