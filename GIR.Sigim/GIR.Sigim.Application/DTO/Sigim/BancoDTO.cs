using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class BancoDTO : BaseDTO
    {
        public string Nome { get; set; }
        public int NumeroRemessa { get; set; }
        public int NumeroRemessaPagamento { get; set; }
        public bool InterfaceEletronica { get; set; }
        public bool Ativo { get; set; }
    }
}