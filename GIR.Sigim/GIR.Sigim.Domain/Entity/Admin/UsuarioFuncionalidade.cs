using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Admin
{
    public class UsuarioFuncionalidade : BaseEntity
    {
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int ModuloId { get; set; }
        public Modulo Modulo { get; set; }
        public string Funcionalidade { get; set; }
        //public ICollection<PerfilFuncionalidade> ListaFuncionalidade { get; set; }
        
        public UsuarioFuncionalidade()
        {
            //this.ListaFuncionalidade = new HashSet<PerfilFuncionalidade>();
            //this.ListaUsuario = new HashSet<Usuario>();
        }
    }
}