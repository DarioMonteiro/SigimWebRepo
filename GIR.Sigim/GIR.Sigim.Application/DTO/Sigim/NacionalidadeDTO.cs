using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class NacionalidadeDTO : BaseDTO
    {
        public string Descricao { get; set; }
        public bool? Automatico { get; set; }
    }
}