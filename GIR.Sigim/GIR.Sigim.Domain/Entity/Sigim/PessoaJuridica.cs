using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class PessoaJuridica : BaseEntity
    {
        public ClienteFornecedor Cliente { get; set; }
        public string Cnpj { get; set; }
    }
}
