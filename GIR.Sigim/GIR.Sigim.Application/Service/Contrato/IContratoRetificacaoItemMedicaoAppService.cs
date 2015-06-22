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
        bool Cancelar(int? contratoRetificacaoItemMedicaoId);
        FileDownloadDTO Exportar(int? contratadoId, int contratoId, int tipoDocumentoId, string numeroDocumento, DateTime dataEmissao, string retencaoContratual, string valorContratadoItem, FormatoExportacaoArquivo formato);
    }
}
