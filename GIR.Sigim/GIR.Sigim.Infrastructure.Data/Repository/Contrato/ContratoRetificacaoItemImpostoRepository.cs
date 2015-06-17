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
    public class ContratoRetificacaoItemImpostoRepository : Repository<ContratoRetificacaoItemImposto>, IContratoRetificacaoItemImpostoRepository
    {
        #region Construtor

        public ContratoRetificacaoItemImpostoRepository(UnitOfWork unitOfWork)
            :base(unitOfWork)
        {

        }

        #endregion

        #region IRepository<TEntity> Members

        public IEnumerable<ContratoRetificacaoItemImposto> RecuperaImpostoPorContratoDadosDaNota(int contratoId,
                                                                                                 int tipoDocumentoId,
                                                                                                 string numeroDocumento,
                                                                                                 DateTime dataEmissao,
                                                                                                 int? contratadoId,
                                                                                                 params Expression<Func<ContratoRetificacaoItemImposto, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);
            if ((contratadoId.HasValue) && (contratadoId.Value > 0))
            {
                set = set.Where(l => l.ContratoId == contratoId &&
                                l.ContratoRetificacaoItem.ListaContratoRetificacaoItemMedicao.Any(c =>
                                    c.TipoDocumentoId == tipoDocumentoId &&
                                    c.NumeroDocumento == numeroDocumento &&
                                    c.DataEmissao == dataEmissao &&
                                    ((c.MultiFornecedorId == contratadoId) || (c.MultiFornecedorId == null && c.Contrato.ContratadoId == contratadoId))) &&
                                (l.ImpostoFinanceiro.Retido == true ||
                                 l.ImpostoFinanceiro.Indireto == true));
            }
            else
            {
                //set = set.Where(l => l.ContratoId == contratoId &&
                //                l.TipoDocumentoId == tipoDocumentoId &&
                //                l.NumeroDocumento == numeroDocumento &&
                //                (l.ImpostoFinanceiro.Retido == true ||
                //                 l.ImpostoFinanceiro.Indireto == true) &&
                //                l.DataEmissao == dataEmissao);
            }
            return set;
        }

        #endregion

    }
}
