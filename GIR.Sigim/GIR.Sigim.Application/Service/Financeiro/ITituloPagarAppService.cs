using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.Filtros.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public interface ITituloPagarAppService : IBaseAppService
    {
        bool ExisteNumeroDocumento(Nullable<DateTime> DataEmissao, Nullable<DateTime> DataVencimento, string NumeroDocumento, int? ClienteId);
        bool EhPermitidoImprimirRelContasPagarTitulo();
        List<RelContasPagarTitulosDTO> ListarPeloFiltroRelContasPagarTitulos(RelContasPagarTitulosFiltro filtro, int? usuarioId, out decimal totalValorTitulo, out decimal totalValorLiquido, out decimal totalValorApropriado);
        //List<RelContasPagarTitulosDTO> ListarPeloFiltroRelContasPagarTitulos(RelContasPagarTitulosFiltro filtro, int? usuarioId, out int totalRegistros, out decimal totalValorTitulo, out decimal totalValorLiquido, out decimal totalValorApropriado);
        List<RelContasPagarTitulosDTO> PaginarPeloFiltroRelContasPagarTitulos(RelContasPagarTitulosFiltro filtro, List<RelContasPagarTitulosDTO> listaRelContasPagarTitulos, out int totalRegistros);
        FileDownloadDTO ExportarRelContasPagarTitulos(RelContasPagarTitulosFiltro filtro, List<RelContasPagarTitulosDTO> listaRelContasPagarTitulos, decimal totalValorTitulo, decimal totalValorLiquido, FormatoExportacaoArquivo formato);
        //FileDownloadDTO ExportarRelContasPagarTitulos(RelContasPagarTitulosFiltro filtro, int? usuarioId, FormatoExportacaoArquivo formato);
    }
}
