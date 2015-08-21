using GIR.Sigim.Domain.Entity.Contrato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Specification;

namespace GIR.Sigim.Domain.Repository.Contrato
{
    public interface IContratoRepository : IRepository<Entity.Contrato.Contrato> 
    {
        void AdicionarItemMedicao(ContratoRetificacaoItemMedicao item);
        void RemoverItemMedicao(ContratoRetificacaoItemMedicao item);
        void AlterarItemMedicao(ContratoRetificacaoItemMedicao item);
        IEnumerable<Domain.Entity.Contrato.Contrato> Pesquisar(ISpecification<Domain.Entity.Contrato.Contrato> specification,
                                                               int pageIndex,
                                                               int pageCount,
                                                               string orderBy,
                                                               bool ascending,
                                                               out int totalRecords,
                                                               params Expression<Func<Domain.Entity.Contrato.Contrato, object>>[] includes);
    }
}
