using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Financeiro
{
    public class TipoCompromissoDTO : BaseDTO
    {
        public string Descricao { get; set; }
        public bool GeraTitulo { get; set; }
        public bool TipoPagar { get; set; }
        public bool TipoReceber { get; set; }
    }
}