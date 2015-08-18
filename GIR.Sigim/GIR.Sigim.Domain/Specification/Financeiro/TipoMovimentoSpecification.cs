using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Specification.Financeiro
{
    public class TipoMovimentoSpecification : BaseSpecification<TipoMovimento>
    {

        public static Specification<TipoMovimento> EhNaoAutomatico()
        {
            Specification<TipoMovimento> specification = new TrueSpecification<TipoMovimento>();

            var directSpecification = new DirectSpecification<TipoMovimento>(l => l.Automatico == false);
            specification &= directSpecification;

            return specification;
        }

    }
}
