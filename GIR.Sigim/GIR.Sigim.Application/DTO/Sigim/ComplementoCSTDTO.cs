using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class ComplementoCSTDTO : BaseDTO
    {
        [Display(Name = "Complemento CST")]
        public string Codigo { get; set; }
        public string Descricao { get; set; }
    }
}