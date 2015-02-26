using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Admin
{
    public class Usuario : BaseEntity
    {
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        [Obsolete("Esta propriedade será removida em uma versão futura. Caso NÃO esteja codificando em um repositório, utilize a propriedade \"Ativo\"")]
        public string Situacao { get; set; }
        public bool Ativo
        {
            get { return Situacao == "A"; }
            set { Situacao = value ? "A" : "I"; }
        }
        public int? ParametrosUsuarioId { get; set; }
        public ParametrosUsuario ParametrosUsuario { get; set; }
        public ICollection<Funcionalidade> ListaFuncionalidade { get; set; }
        public ICollection<Perfil> ListaPerfil { get; set; }
        public ICollection<UsuarioCentroCusto> ListaUsuarioCentroCusto { get; set; }

        public Usuario()
        {
            this.ListaFuncionalidade = new HashSet<Funcionalidade>();
            this.ListaPerfil = new HashSet<Perfil>();
            this.ListaUsuarioCentroCusto = new HashSet<UsuarioCentroCusto>();
        }
    }
}