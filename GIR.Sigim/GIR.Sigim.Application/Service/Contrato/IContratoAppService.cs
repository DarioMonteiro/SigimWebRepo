using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Contrato;
using GIR.Sigim.Application.Filtros.Contrato;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Service.Contrato
{
    public interface IContratoAppService : IBaseAppService 
    {
        List<ContratoDTO> ListarPeloFiltro(MedicaoContratoFiltro filtro,int? idUsuario, out int totalRegistros);
        List<ContratoDTO> ListarPeloFiltro(LiberacaoContratoFiltro filtro, int? idUsuario, out int totalRegistros);
        ContratoDTO ObterPeloId(int? id, int? idUsuario);
        bool EhContratoAssinado(ContratoDTO dto);
        bool EhContratoExistente(ContratoDTO dto);
        bool EhContratoComCentroCustoAtivo(ContratoDTO dto);
        List<ContratoRetificacaoProvisaoDTO> ObterListaCronogramaPorContratoEhRetificacaoItem(int contratoId, int contratoRetificacaoItemId);
        List<ContratoRetificacaoItemMedicaoDTO> ObtemMedicaoPorSequencialItem(int contratoId, int sequencialItem);
        bool ExisteContratoRetificacaoProvisao(List<ContratoRetificacaoProvisaoDTO> listaContratoRetificacaoProvisao);
        bool ExisteMedicao(ContratoRetificacaoItemMedicaoDTO dto);
        ContratoRetificacaoItemMedicaoDTO ObtemMedicaoPorId(int contratoId, int contratoRetificacaoItemMedicaoId);
        bool EhValidoParametrosVisualizacaoMedicao(int? contratoId, int? tipoDocumentoId, string numeroDocumento, Nullable<DateTime> dataEmissao, int? contratadoId);
        List<ContratoRetificacaoItemMedicaoDTO> RecuperaMedicaoPorDadosDaNota(int contratoId, int tipoDocumentoId, string numeroDocumento, DateTime dataEmissao, int? contratadoId);
        bool ExisteNumeroDocumento(Nullable<DateTime> dataEmissao, string numeroDocumento, int? contratadoId);
        FileDownloadDTO ExportarMedicao(int contratoId,int? contratadoId,int tipoDocumentoId,string numeroDocumento,DateTime dataEmissao,string retencaoContratual,string valorContratadoItem,FormatoExportacaoArquivo formato);
        bool ExcluirMedicao(int? contratoId, int? contratoRetificacaoItemMedicaoId);
        bool SalvarMedicao(ContratoRetificacaoItemMedicaoDTO dto);
        bool EhPermitidoSalvarMedicao();
        bool EhPermitidoDeletarMedicao();
        bool EhPermitidoImprimirMedicao();
        List<ContratoDTO> PesquisarContratosPeloFiltro(ContratoPesquisaFiltro filtro, out int totalRegistros);
        void PreencherResumo(ContratoDTO contrato, ContratoRetificacaoItemDTO contratoRetificacaoItemSelecionado, ResumoLiberacaoDTO resumo);
    }
}
