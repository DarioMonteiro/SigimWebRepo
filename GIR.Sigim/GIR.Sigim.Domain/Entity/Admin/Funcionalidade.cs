using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Admin
{
    public class Funcionalidade : BaseEntity
    {
        public string Descricao { get; set; }
        public string ChaveAcesso { get; set; }
        public bool Ativo { get; set; }
        public int ModuloId { get; set; }
        public Modulo Modulo { get; set; }
        public ICollection<Usuario> ListaUsuario { get; set; }
        public ICollection<Perfil> ListaPerfil { get; set; }

        public Funcionalidade()
        {
            this.ListaUsuario = new HashSet<Usuario>();
            this.ListaPerfil = new HashSet<Perfil>();
        }
    }
}