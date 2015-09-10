using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class NaturezaReceitaDTO : BaseDTO
    {
        [Display(Name = "Natureza da receita")]
        public string Codigo { get; set; }
        public string Descricao { get; set; }
    }
}