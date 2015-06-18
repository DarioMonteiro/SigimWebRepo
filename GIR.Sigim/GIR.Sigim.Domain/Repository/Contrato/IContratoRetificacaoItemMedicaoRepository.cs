using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Domain.Repository.Contrato
{
    public interface IContratoRetificacaoItemMedicaoRepository : IRepository<ContratoRetificacaoItemMedicao>
    {
        IEnumerable<ContratoRetificacaoItemMedicao> RecuperaMedicaoPorContratoDadosDaNota(int contratoId,
                                                                                          int tipoDocumentoId,
                                                                                          string numeroDocumento,
                                                                                          DateTime dataEmissao,
                                                                                          int? contratadoId,
                                                                                          params Expression<Func<ContratoRetificacaoItemMedicao, object>>[] includes);
    }
}
