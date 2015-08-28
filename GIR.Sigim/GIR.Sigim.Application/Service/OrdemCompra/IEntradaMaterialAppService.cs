using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.OrdemCompras;

namespace GIR.Sigim.Application.Service.OrdemCompra
{
    public interface IEntradaMaterialAppService : IBaseAppService
    {
        List<EntradaMaterialDTO> ListarPeloFiltro(EntradaMaterialFiltro filtro, out int totalRegistros);
        EntradaMaterialDTO ObterPeloId(int? id);
        bool CancelarEntrada(int? id, string motivo);
        FileDownloadDTO Exportar(int? id, FormatoExportacaoArquivo formato);
        bool EhPermitidoSalvar(EntradaMaterialDTO dto);
        bool EhPermitidoCancelar(EntradaMaterialDTO dto);
        bool EhPermitidoImprimir(EntradaMaterialDTO dto);
        bool EhPermitidoLiberarTitulos(EntradaMaterialDTO dto);
        bool EhPermitidoAdicionarItem(EntradaMaterialDTO dto);
        bool EhPermitidoCancelarItem(EntradaMaterialDTO dto);
        bool EhPermitidoEditarItem(EntradaMaterialDTO dto);

        bool EhPermitidoEditarCentroCusto(EntradaMaterialDTO entradaMaterial);
        bool EhPermitidoEditarFornecedor(EntradaMaterialDTO entradaMaterial);
        bool ExisteEstoqueParaCentroCusto(string codigoCentroCusto);
        bool ExisteMovimentoNoEstoque(EntradaMaterialDTO dto);
        bool HaPossibilidadeCancelamentoEntradaMaterial(int? entradaMaterialId);
    }
}