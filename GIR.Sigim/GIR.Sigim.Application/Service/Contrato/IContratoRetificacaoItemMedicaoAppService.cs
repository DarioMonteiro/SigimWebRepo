using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Contrato;
using GIR.Sigim.Domain.Entity.Contrato;

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
                                              ref decimal valorTotalLiberado);

        bool ExisteNumeroDocumento(Nullable<DateTime> dataEmissao, string numeroDocumento, int? contratadoId);
        bool Salvar(ContratoRetificacaoItemMedicaoDTO dto);
        List<ContratoRetificacaoItemMedicaoDTO> ObtemPorSequencialItem(int contratoId, int sequencialItem);
        ContratoRetificacaoItemMedicaoDTO ObterPeloId(int contratoRetificacaoItemMedicaoId);
        bool EhValidaMedicaoRecuperada(ContratoRetificacaoItemMedicaoDTO dto);
        bool Cancelar(int? contratoRetificacaoItemMedicaoId);
    }
}
