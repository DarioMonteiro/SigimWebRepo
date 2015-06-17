using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Repository.Contrato;

namespace GIR.Sigim.Infrastructure.Data.Repository.Contrato
{
    public class ContratoRetificacaoItemMedicaoRepository : Repository<ContratoRetificacaoItemMedicao>, IContratoRetificacaoItemMedicaoRepository
    {
        #region Construtor

        public ContratoRetificacaoItemMedicaoRepository(UnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }
        #endregion

        #region IRepository<TEntity> Members

        public IEnumerable<ContratoRetificacaoItemMedicao> RecuperaMedicaoPorContratoDadosDaNota(int contratoId,
                                                                                                 int tipoDocumentoId,
                                                                                                 string numeroDocumento,
                                                                                                 DateTime dataEmissao,
                                                                                                 int? contratadoId,
                                                                                                 params Expression<Func<ContratoRetificacaoItemMedicao, object>>[] includes)
                                                                                          
        {
            var set = CreateSetAsQueryable(includes);
            if ((contratadoId.HasValue) && (contratadoId.Value > 0))
            {
                set = set.Where(l => l.ContratoId == contratoId &&
                                l.TipoDocumentoId == tipoDocumentoId &&
                                l.NumeroDocumento == numeroDocumento &&
                                l.DataEmissao == dataEmissao &&
                                ((l.MultiFornecedorId == contratadoId) ||
                                (l.MultiFornecedorId == null && l.Contrato.ContratadoId == contratadoId)));
            }
            else
            {
                set = set.Where(l => l.ContratoId == contratoId &&
                                l.TipoDocumentoId == tipoDocumentoId &&
                                l.NumeroDocumento == numeroDocumento &&
                                l.DataEmissao == dataEmissao);
            }
            return set;
        }

        #endregion
    }
}
