using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface INacionalidadeAppService
    {
        List<NacionalidadeDTO> ListarTodos();
        List<NacionalidadeDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros);
        NacionalidadeDTO ObterPeloId(int? id);
        bool Salvar(NacionalidadeDTO dto);
        bool Deletar(int? id);
    }
}