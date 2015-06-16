using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Contrato;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Service.Contrato
{
    public interface IContratoRetificacaoItemMedicaoAppService : IBaseAppService
    {
        void ObterQuantidadesEhValoresMedicao(int contratoId,
                                              int sequenciaItem,
                                              int sequencialCronograma,
                                              ref decimal quantidadeTotalMedido,
                                              ref decimal valorTotalMedido,
                                              ref decimal quantidadeTotalLiberado,
                                              ref decimal valorTotalLiberado,
                                              ref decimal quantidadeTotalMedidaLiberada,
                                              ref decimal valorTotalMedidoLiberado);

        bool ExisteNumeroDocumento(Nullable<DateTime> dataEmissao, string numeroDocumento, int? contratadoId);
        bool Salvar(ContratoRetificacaoItemMedicaoDTO dto);
        List<ContratoRetificacaoItemMedicaoDTO> ObtemPorSequencialItem(int contratoId, int sequencialItem);
        ContratoRetificacaoItemMedicaoDTO ObterPeloId(int contratoRetificacaoItemMedicaoId);
        bool EhValidaMedicaoRecuperada(ContratoRetificacaoItemMedicaoDTO dto);
        bool Cancelar(int? contratoRetificacaoItemMedicaoId);
        bool EhValidaVisualizacaoMedicao(int? contratoId, int? tipoDocumentoId, string numeroDocumento, Nullable<DateTime> dataEmissao, int? contratadoId);
        List<ContratoRetificacaoItemMedicaoDTO> RecuperaMedicaoPorContratoDadosDaNota(int contratoId, int tipoDocumentoId, string numeroDocumento, DateTime dataEmissao, int? contratadoId);
        FileDownloadDTO Exportar(int? contratadoId, int contratoId, int tipoDocumentoId, string numeroDocumento, DateTime dataEmissao, FormatoExportacaoArquivo formato);
        //bool EhValidaImpressao(int? contratadoId, int? contratoId, int? tipoDocumentoId, string numeroDocumento, Nullable<DateTime> dataEmissao);
    }
}
