using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class PessoaFisicaDTO : BaseDTO
    {
        public ClienteFornecedorDTO ClienteFornecedor { get; set; }
        public string Cpf { get; set; }
    }
}
