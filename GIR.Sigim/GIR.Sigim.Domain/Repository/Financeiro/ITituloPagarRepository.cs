using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Specification;

namespace GIR.Sigim.Domain.Repository.Financeiro
{
    public interface ITituloPagarRepository : IRepository<TituloPagar>
    {
        void RemoverTituloPagarAdiantamento(TituloPagarAdiantamento titulo);
        void RemoverApropriacaoAdiantamento(ApropriacaoAdiantamento apropriacao);
        void RemoverImpostoPagar(ImpostoPagar impostoPagar);
        void RemoverTituloPagar(TituloPagar titulo);
        void RemoverApropriacao(Apropriacao apropriacao);
        IEnumerable<TituloPagar> ListarPeloFiltroComUnion(ISpecification<TituloPagar> specification, ISpecification<TituloPagar> specification1, params Expression<Func<TituloPagar, object>>[] includes);
    }
}