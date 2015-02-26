using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO
{
    public class BaseDTO
    {
        [Display(Name = "Código")]
        public int? Id { get; set; }
    }
}