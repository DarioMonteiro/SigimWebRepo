using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Specification.Sigim
{
    public class ClienteFornecedorSpecification : BaseSpecification<ClienteFornecedor>
    {
        public static Specification<ClienteFornecedor> NomeContem(string nome)
        {
            Specification<ClienteFornecedor> specification = new TrueSpecification<ClienteFornecedor>();

            if (!string.IsNullOrEmpty(nome))
            {
                var directSpecification = new DirectSpecification<ClienteFornecedor>(l => l.Nome.Contains(nome));
                specification &= directSpecification;
            }

            return specification;
        }

        public static Specification<ClienteFornecedor> EhAtivo()
        {
            return new DirectSpecification<ClienteFornecedor>(l => l.Situacao == "A" || l.Situacao == null);
        }

        public static Specification<ClienteFornecedor> EhClienteContrato()
        {
            return new DirectSpecification<ClienteFornecedor>(l => l.ClienteContrato == "S");
        }

    }
}
