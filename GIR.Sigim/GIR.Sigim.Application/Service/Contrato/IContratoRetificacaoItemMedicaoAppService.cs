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
        bool ExisteNumeroDocumento(Nullable<DateTime> dataEmissao, string numeroDocumento, int? contratadoId);
        bool Salvar(ContratoRetificacaoItemMedicaoDTO dto);
        bool Cancelar(int? contratoRetificacaoItemMedicaoId);
    }
}
