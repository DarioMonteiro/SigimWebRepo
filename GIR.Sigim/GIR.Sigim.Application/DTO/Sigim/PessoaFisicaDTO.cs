using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class PessoaFisicaDTO : BaseDTO
    {
        public ClienteFornecedorDTO ClienteFornecedor { get; set; }
        [Display(Name = "C.P.F")]
        public string Cpf { get; set; }
    }
}
