using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class ClienteFornecedor : BaseEntity
    {
        public string Nome { get; set; }
        public ICollection<CentroCustoEmpresa> ListaCentroCustoEmpresa { get; set; }
        public ICollection<OrdemCompra.ParametrosOrdemCompra> ListaParametrosOrdemCompra { get; set; }
        public ICollection<Contrato.Contrato> ListaContratoContratante { get; set; }
        public ICollection<Contrato.Contrato> ListaContratoContratado { get; set; }
        public ICollection<Contrato.Contrato> ListaContratoInterveniente { get; set; }
        [Obsolete("Esta propriedade será removida em uma versão futura. Caso NÃO esteja codificando em um repositório, utilize a propriedade \"Ativo\"")]
        public string Situacao { get; set; }
        public bool Ativo
        {
            get { return Situacao == "A"; }
            set { Situacao = value ? "A" : "I"; }
        }

        public ClienteFornecedor()
        {
            this.ListaCentroCustoEmpresa = new HashSet<CentroCustoEmpresa>();
            this.ListaParametrosOrdemCompra = new HashSet<OrdemCompra.ParametrosOrdemCompra>();
        }
    }
}