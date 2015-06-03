using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface IFonteNegocioAppService
    {
        List<FonteNegocioDTO> ListarTodos();
        List<FonteNegocioDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros);
        FonteNegocioDTO ObterPeloId(int? id);
        bool Salvar(FonteNegocioDTO dto);
        bool Deletar(int? id);
    }
}