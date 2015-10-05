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
        List<OrdemCompraItemDTO> ListarItensDeOrdemCompraLiberadaComSaldo(int? entradaMaterialId);
        bool AdicionarItens(int? entradaMaterialId, int?[] itens);
        bool RemoverItens(int? entradaMaterialId, int?[] itens);
        List<EntradaMaterialItemDTO> ListarItens(int? entradaMaterialId);
        List<FreteDTO> ListarFretePendente(int? entradaMaterialId);
        bool CancelarEntrada(int? id, string motivo);
        bool LiberarTitulos(int? id);
        FileDownloadDTO Exportar(int? id, FormatoExportacaoArquivo formato);
        bool EhPermitidoSalvar(EntradaMaterialDTO dto);
        bool EhPermitidoCancelar(EntradaMaterialDTO dto);
        bool EhPermitidoImprimir(EntradaMaterialDTO dto);
        bool EhPermitidoLiberarTitulos(EntradaMaterialDTO dto);
        bool EhPermitidoAdicionarItem(EntradaMaterialDTO dto);
        bool EhPermitidoRemoverItem(EntradaMaterialDTO dto);
        bool EhPermitidoEditarItem(EntradaMaterialDTO dto);
        bool EhPermitidoAdicionarImposto(EntradaMaterialDTO entradaMaterial);
        bool EhPermitidoRemoverImposto(EntradaMaterialDTO entradaMaterial);
        bool EhPermitidoEditarImposto(EntradaMaterialDTO entradaMaterial);

        bool EhPermitidoEditarCentroCusto(EntradaMaterialDTO entradaMaterial);
        bool EhPermitidoEditarFornecedor(EntradaMaterialDTO entradaMaterial);
        bool ExisteEstoqueParaCentroCusto(string codigoCentroCusto);
        bool ExisteMovimentoNoEstoque(EntradaMaterialDTO dto);
        bool HaPossibilidadeCancelamentoEntradaMaterial(int? entradaMaterialId);
        bool HaPossibilidadeLiberacaoTitulos(int? entradaMaterialId);

        bool EhDataEntradaMaterialValida(int? id, Nullable<DateTime> data);
        bool EhNumeroNotaFiscalValido(EntradaMaterialDTO dto);
        bool EhDataEmissaoNotaValida(EntradaMaterialDTO entradaMaterial);
    }
}