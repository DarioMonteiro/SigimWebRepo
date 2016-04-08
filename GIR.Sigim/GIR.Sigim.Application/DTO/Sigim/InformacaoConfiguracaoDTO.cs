using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class InformacaoConfiguracaoDTO
    {
        public bool LogGirCliente { get; set; }
        public string HostName { get; set; }
        public string NomeDoBancoDeDados { get; set; }
        public string Servidor { get; set; }
    }
}
