using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Admin
{
    public class Perfil : BaseEntity
    {
        public string Descricao { get; set; }
        public int ModuloId { get; set; }
        public Modulo Modulo { get; set; }
        public ICollection<PerfilFuncionalidade> ListaFuncionalidade { get; set; }
        //public ICollection<Usuario> ListaUsuario { get; set; }

        public Perfil()
        {
            this.ListaFuncionalidade = new HashSet<PerfilFuncionalidade>();
            //this.ListaUsuario = new HashSet<Usuario>();
        }
    }
}