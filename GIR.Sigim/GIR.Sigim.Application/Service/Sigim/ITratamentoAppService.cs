using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface ITratamentoAppService
    {
        List<TratamentoDTO> ListarTodos();
        List<TratamentoDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros);
        TratamentoDTO ObterPeloId(int? id);
        bool Salvar(TratamentoDTO dto);
        bool Deletar(int? id);
    }
}