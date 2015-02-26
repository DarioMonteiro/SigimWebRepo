using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class UnidadeMedidaDTO : BaseDTO
    {
        public string Sigla { get; set; }
        public string Descricao { get; set; }
    }
}