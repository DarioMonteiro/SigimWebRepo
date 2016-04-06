using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Admin
{
    public class ModuloDTO : BaseDTO 
    {
        public string Nome { get; set; }
        public string NomeCompleto { get; set; }
        public string ChaveAcesso { get; set; }
        public string Versao { get; set; }
        public bool Bloqueio { get; set; }      
    }
}