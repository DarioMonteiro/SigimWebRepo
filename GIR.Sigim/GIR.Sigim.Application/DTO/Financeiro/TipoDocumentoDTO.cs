using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Financeiro
{
    public class TipoDocumentoDTO : BaseDTO
    {
        public string Sigla { get; set; }
        public string Descricao { get; set; }
    }
}
