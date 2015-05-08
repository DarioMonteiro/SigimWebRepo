using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Contrato;

namespace GIR.Sigim.Application.Service.Contrato
{
    public interface IContratoRetificacaoItemMedicaoAppService : IBaseAppService
    {
        void ObterQuantidadesEhValoresMedicao(int ContratoRetificacaoItemId,
                                              int ContratoRetificacaoItemCronogramaId,
                                              ref decimal QuantidadeTotalMedido,
                                              ref decimal ValorTotalMedido,
                                              ref decimal QuantidadeTotalLiberado,
                                              ref decimal ValorTotalLiberado);

        int SetaSituacaoAguardandoAprovacao();

        bool ExisteNumeroDocumento(Nullable<DateTime> DataEmissao, string NumeroDocumento, int? ContratadoId);
    }
}
