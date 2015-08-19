using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.Estoque
{
    public class Estoque : BaseEntity
    {
        public string Descricao { get; set; }
        public string CodigoCentroCusto { get; set; }
        public CentroCusto CentroCusto { get; set; }
        //Endereco
        [Obsolete("Esta propriedade será removida em uma versão futura. Caso NÃO esteja codificando em um repositório, utilize a propriedade \"Ativo\"")]
        public string Situacao { get; set; }
        public bool Ativo
        {
            get { return Situacao == "A"; }
            set { Situacao = value ? "A" : "I"; }
        }

        public ICollection<EstoqueMaterial> ListaEstoqueMaterial { get; set; }

        public Estoque()
        {
            this.ListaEstoqueMaterial = new HashSet<EstoqueMaterial>();
        }
    }
}