using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Admin
{
    public class UsuarioDTO : BaseDTO
    {
        public string Nome { get; set; }
        public string Login { get; set; }
        //TODO: Inverter para Ativo
        public bool Inativo { get; set; }
        public byte[] AssinaturaEletronica { get; set; }

        public List<UsuarioFuncionalidadeDTO> ListaUsuarioFuncionalidade { get; set; }
        public List<UsuarioPerfilDTO> ListaUsuarioPerfil { get; set; }

        public UsuarioDTO()
        {
            this.ListaUsuarioFuncionalidade = new List<UsuarioFuncionalidadeDTO>();
            this.ListaUsuarioPerfil = new List<UsuarioPerfilDTO>(); 
        }

    }
}