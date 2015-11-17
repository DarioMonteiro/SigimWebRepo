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
    public interface IRateioAutomaticoAppService
    {
        List<RateioAutomaticoDTO> ListarPeloTipoRateio(int TipoRateioId);
        bool Salvar(int TipoRateioId, List<RateioAutomaticoDTO> listaDto);
        bool Deletar(int TipoRateioId);
        bool EhPermitidoSalvar();
        bool EhPermitidoDeletar();
        bool EhPermitidoImprimir();
        FileDownloadDTO ExportarRelRateioAutomatico(int? id, FormatoExportacaoArquivo formato);
    }
}