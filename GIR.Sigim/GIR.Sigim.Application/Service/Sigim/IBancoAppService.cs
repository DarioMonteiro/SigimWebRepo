using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.Sigim;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface IBancoAppService
    {
        List<BancoDTO> ListarPeloFiltro(BancoFiltro filtro, out int totalRegistros);
        List<BancoDTO> ListarTodos();
        BancoDTO ObterPeloId(int? Id);
        bool Salvar(BancoDTO dto);
        bool Deletar(int? Id);

    }
}