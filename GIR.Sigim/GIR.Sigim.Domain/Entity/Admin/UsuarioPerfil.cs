using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Admin
{
    public class UsuarioPerfil : BaseEntity
    {
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int ModuloId { get; set; }
        public Modulo Modulo { get; set; }
        public int PerfilId { get; set; }
        public Perfil Perfil { get; set; }
        

        public UsuarioPerfil()
        {
            //this.ListaFuncionalidade = new HashSet<PerfilFuncionalidade>();
            //this.ListaUsuario = new HashSet<Usuario>();
        }
    }
}