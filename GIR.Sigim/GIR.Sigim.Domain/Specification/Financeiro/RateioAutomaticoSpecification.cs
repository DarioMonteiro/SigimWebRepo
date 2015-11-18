using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Specification.Financeiro
{
    public class RateioAutomaticoSpecification : BaseSpecification<RateioAutomatico>
    {
        public static Specification<RateioAutomatico> PertenceAoTipoRateio(int? tipoRateioId)
        {
            Specification<RateioAutomatico> specification = new TrueSpecification<RateioAutomatico>();

            if (tipoRateioId.HasValue)
            {
                var directSpecification = new DirectSpecification<RateioAutomatico>(l => l.TipoRateioId == tipoRateioId);
                specification &= directSpecification;
            }

            return specification;
        }


    }
}
