using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class ComplementoNaturezaOperacaoDTO : BaseDTO
    {
        [Display(Name = "Complemento natureza operação")]
        public string Codigo { get; set; }
        public string Descricao { get; set; }
    }
}