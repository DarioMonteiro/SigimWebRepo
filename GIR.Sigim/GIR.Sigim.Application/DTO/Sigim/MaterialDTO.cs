using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class MaterialDTO : BaseDTO
    {
        public string Descricao { get; set; }
        public string SiglaUnidadeMedida { get; set; }
    }
}