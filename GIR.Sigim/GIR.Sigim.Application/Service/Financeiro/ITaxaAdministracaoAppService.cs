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
    public interface ITaxaAdministracaoAppService
    {
        List<TaxaAdministracaoDTO> ListarPeloCentroCustoCliente(string  CentroCustoId, int ClienteId);
        List<TaxaAdministracaoDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros);
        bool Salvar(string CentroCustoId, int ClienteId, List<TaxaAdministracaoDTO> listaDto);
        bool Deletar(string CentroCustoId, int ClienteId);
    }
}