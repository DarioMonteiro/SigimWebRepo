using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public interface IImpostoFinanceiroAppService
    {
        List<ImpostoFinanceiroDTO> ListarTodos();
        List<ImpostoFinanceiroDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros);
        ImpostoFinanceiroDTO ObterPeloId(int? id);
        bool Salvar(ImpostoFinanceiroDTO dto);
        bool Deletar(int? id);
        List<ItemListaDTO> ListarOpcoesPeriodicidade();
        List<ItemListaDTO> ListarOpcoesFimDeSemana();
        List<ItemListaDTO> ListarOpcoesFatoGerador();
        bool EhPermitidoSalvar();
        bool EhPermitidoDeletar();
        //bool EhPermitidoImprimir();
    }
}