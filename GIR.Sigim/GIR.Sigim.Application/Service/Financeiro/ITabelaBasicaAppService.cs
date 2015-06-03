using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Application.Enums;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public interface ITabelaBasicaAppService
    {
        List<TabelaBasicaDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros, int? tipoTabela);
        TabelaBasicaDTO ObterPeloId(int? id, int? tipoTabela);
        bool Salvar(TabelaBasicaDTO dto);
        bool Deletar(int? id, int tipoTabela);
    }
}
