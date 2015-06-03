using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface IProfissaoAppService
    {
        List<ProfissaoDTO> ListarTodos();
        List<ProfissaoDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros);
        ProfissaoDTO ObterPeloId(int? id);
        bool Salvar(ProfissaoDTO dto);
        bool Deletar(int? id);
    }
}