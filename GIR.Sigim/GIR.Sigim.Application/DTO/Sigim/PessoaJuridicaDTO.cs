using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class PessoaJuridicaDTO : BaseDTO 
    {
        public ClienteFornecedorDTO ClienteFornecedor { get; set; }
        [Display(Name = "C.N.P.J")]
        public string Cnpj { get; set; }
        public string NomeFantasia { get; set; }
        public string InscricaoEstadual { get; set; }
    }
}
