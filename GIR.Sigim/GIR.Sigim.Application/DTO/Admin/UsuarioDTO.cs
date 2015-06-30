using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Admin
{
    public class UsuarioDTO
    {
        public string Nome { get; set; }
        public string Login { get; set; }
        //TODO: Inverter para Ativo
        public bool Inativo { get; set; }
        public byte[] AssinaturaEletronica { get; set; }
    }
}