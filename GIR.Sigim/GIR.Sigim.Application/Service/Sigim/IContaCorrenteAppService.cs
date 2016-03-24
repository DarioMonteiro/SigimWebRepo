using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.Sigim;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface IContaCorrenteAppService
    {
        List<ContaCorrenteDTO> ListarPeloFiltro(ContaCorrenteFiltro filtro, int? idUsuario, out int totalRegistros);
        ContaCorrenteDTO ObterPeloId(int? Id);
        List<ItemListaDTO> ListarTipo();
        List<ContaCorrenteDTO> ListarAtivosPorBanco(int? bancoId);
        bool Salvar(ContaCorrenteDTO dto);
        bool Deletar(int? id);

    }
}