using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Admin
{
    public class PerfilFuncionalidade : BaseEntity
    {
        public int? PerfilId { get; set; }
        public Perfil Perfil { get; set; }
        public string Funcionalidade { get; set; }
        //public ICollection<Perfil> ListaPerfil { get; set; }
                
        public PerfilFuncionalidade()
        {
            //this.ListaPerfil = new HashSet<Perfil>();  
        }
    }
}