using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;


namespace GIR.Sigim.Domain.Specification.Sigim
{
    public class BancoSpecification : BaseSpecification<Banco>
    {
        public static Specification<Banco> EhBanco()
        {
            Specification<Banco> specification = new TrueSpecification<Banco>();

            return new DirectSpecification<Banco>(l => l.Id != 999);
        }
        
    }
}
