using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.Sigim;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface IUnidadeMedidaAppService
    {
        List<UnidadeMedidaDTO> ListarPeloFiltro(UnidadeMedidaFiltro filtro, out int totalRegistros);
        List<UnidadeMedidaDTO> ListarTodos();    
        UnidadeMedidaDTO ObterPeloCodigo(string sigla);
        bool Salvar(UnidadeMedidaDTO dto);
        bool Deletar(string sigla);
        bool EhPermitidoSalvar();
        bool EhPermitidoDeletar();
        bool EhPermitidoImprimir();
    }
}