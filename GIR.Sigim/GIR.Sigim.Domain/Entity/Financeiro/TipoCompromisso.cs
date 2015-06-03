using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Contrato; 

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class TipoCompromisso : BaseEntity
    {
        public string Descricao { get; set; }
        public bool? GeraTitulo { get; set; }
        //TODO: Juntar as infomações dos campos TipoPagar e TipoReceber em um único campo
        public bool? TipoPagar { get; set; }
        public bool? TipoReceber { get; set; }
        public ICollection<ParametrosOrdemCompra> ListaParametrosOrdemCompra { get; set; }
        public ICollection<ContratoRetificacao> ListaContratoRetificacao { get; set; }
        public ICollection<ContratoRetificacaoItem> ListaContratoRetificacaoItem { get; set; }
        public ICollection<TituloPagar> ListaTituloPagar { get; set; }
        public ICollection<TituloReceber> ListaTituloReceber { get; set; }
        
        public TipoCompromisso()
        {
            this.ListaParametrosOrdemCompra = new HashSet<ParametrosOrdemCompra>();
            this.ListaContratoRetificacao = new HashSet<ContratoRetificacao>();
            this.ListaContratoRetificacaoItem = new HashSet<ContratoRetificacaoItem>();
            this.ListaTituloPagar = new HashSet<TituloPagar>();
            this.ListaTituloReceber = new HashSet<TituloReceber>();
        }
    }
}