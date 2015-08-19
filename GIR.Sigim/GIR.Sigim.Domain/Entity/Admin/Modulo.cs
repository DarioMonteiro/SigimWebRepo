using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Admin
{
    public class Modulo : BaseEntity
    {
        public string Nome { get; set; }
        public string NomeCompleto { get; set; }
        public string ChaveAcesso { get; set; }
        public string Versao { get; set; }
        public ICollection<Perfil> ListaPerfil { get; set; }
        public ICollection<UsuarioCentroCusto> ListaUsuarioCentroCusto { get; set; }

        public Modulo()
        {
            this.ListaPerfil = new HashSet<Perfil>();
            this.ListaUsuarioCentroCusto = new HashSet<UsuarioCentroCusto>();
        }
    }
}