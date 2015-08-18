using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface IFormaRecebimentoAppService
    {
        List<FormaRecebimentoDTO> ListarTodos();
        List<FormaRecebimentoDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros);
        FormaRecebimentoDTO ObterPeloId(int? id);
        List<ItemListaDTO> ListarTipoRecebimento();
        bool Salvar(FormaRecebimentoDTO dto);
        bool Deletar(int? id);
        bool EhPermitidoSalvar();
        bool EhPermitidoDeletar();
        bool EhPermitidoImprimir();
    }
}