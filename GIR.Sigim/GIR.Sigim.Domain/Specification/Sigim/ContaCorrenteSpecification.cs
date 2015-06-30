using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;


namespace GIR.Sigim.Domain.Specification.Sigim
{
    public class ContaCorrenteSpecification : BaseSpecification<ContaCorrente>
    {
        public static Specification<ContaCorrente> PertenceAoBanco(int? bancoId)
        {
            Specification<ContaCorrente> specification = new TrueSpecification<ContaCorrente>();
        
            if (bancoId.HasValue)
                return new DirectSpecification<ContaCorrente>(l => l.BancoId == bancoId);
                
            else
                return new DirectSpecification<ContaCorrente>(l => l.BancoId != 999);           

        }
       
    }
}
