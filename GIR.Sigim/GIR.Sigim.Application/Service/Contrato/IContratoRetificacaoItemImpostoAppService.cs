using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Application.Service.Contrato
{
    public interface IContratoRetificacaoItemImpostoAppService : IBaseAppService
    {
        List<ContratoRetificacaoItemImposto> RecuperaImpostoPorContratoDadosDaNota(int contratoId, 
                                                                                   int tipoDocumentoId, 
                                                                                   string numeroDocumento, 
                                                                                   DateTime dataEmissao,
                                                                                   int? contratadoId,
                                                                                   params Expression<Func<ContratoRetificacaoItemImposto, object>>[] includes);
    }
}
