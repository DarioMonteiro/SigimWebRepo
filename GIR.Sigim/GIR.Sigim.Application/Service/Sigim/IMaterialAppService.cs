using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Orcamento;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.Sigim;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface IMaterialAppService
    {
        List<MaterialDTO> ListarPeloFiltro(MaterialFiltro filtro, out int totalRegistros);
        List<MaterialDTO> ListarAtivosPeloCentroCustoEDescricao(string codigoCentroCusto, string descricao);
        //List<MaterialDTO> ListarAtivosPeloTipoTabelaPropria(string descricao);
        List<MaterialDTO> PesquisarMaterial(MaterialPesquisaFiltro filtro);
        List<OrcamentoComposicaoItemDTO> ListarOrcamentoComposicaoItem(int? materialId, string codigoCentroCusto, string codigoClasse, out bool possuiInterfaceOrcamento);
    }
}