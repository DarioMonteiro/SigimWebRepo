using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;


namespace GIR.Sigim.Domain.Specification.Sigim
{
    public class AgenciaSpecification : BaseSpecification<Agencia>
    {
        public static Specification<Agencia> PertenceAoBanco(int? bancoId)
        {
            Specification<Agencia> specification = new TrueSpecification<Agencia>();
        
            if (bancoId.HasValue)
                return new DirectSpecification<Agencia>(l => l.BancoId == bancoId);
                
            else            
                return new DirectSpecification<Agencia>(l => l.BancoId != 999);           

        }
       
    }
}
