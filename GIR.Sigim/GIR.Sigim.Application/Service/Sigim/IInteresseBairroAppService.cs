using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface IInteresseBairroAppService
    {
        List<InteresseBairroDTO> ListarTodos();
        List<InteresseBairroDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros);
        InteresseBairroDTO ObterPeloId(int? id);
        bool Salvar(InteresseBairroDTO dto);
        bool Deletar(int? id);
    }
}