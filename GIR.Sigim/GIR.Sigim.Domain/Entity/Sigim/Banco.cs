using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class Banco : BaseEntity
    {
        public string Nome { get; set; }
        [Obsolete("Esta propriedade será removida em uma versão futura. Caso NÃO esteja codificando em um repositório, utilize a propriedade \"Ativo\"")]
        public string Situacao { get; set; }
        public bool Ativo
        {
            get { return Situacao == "A"; }
            set { Situacao = value ? "A" : "I"; }
        }
        public int NumeroRemessa { get; set; }
        public int NumeroRemessaPagamento { get; set; }
        public bool InterfaceEletronica { get; set; }
        public ICollection<BancoLayout> ListaBancoLayout { get; set; }

        public Banco()
        {
            this.ListaBancoLayout = new HashSet<BancoLayout>();
        }
    }
}