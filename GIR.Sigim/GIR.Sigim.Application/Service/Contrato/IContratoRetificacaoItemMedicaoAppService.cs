using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Application.Filtros.Contrato;
using GIR.Sigim.Application.DTO.Contrato;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Service.Contrato
{
    public interface IContratoRetificacaoItemMedicaoAppService : IBaseAppService
    {
        string MedicaoToXML(ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao);
        List<RelNotaFiscalLiberadaDTO> ListarPeloFiltroRelNotaFiscalLiberada(RelNotaFiscalLiberadaFiltro filtro, int? idUsuario, out int totalRegistros);
        FileDownloadDTO ExportarRelNotaFiscalLiberada(RelNotaFiscalLiberadaFiltro filtro, int? usuarioId, FormatoExportacaoArquivo formato);
        bool EhPermitidoImprimir();
    }
}
