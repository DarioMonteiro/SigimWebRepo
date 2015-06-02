using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public class Cotacao : BaseEntity
    {
        public DateTime Data { get; set; }
        public SituacaoCotacao Situacao { get; set; }
        public string Observacao { get; set; }
        public DateTime DataCadastro { get; set; }
        //TODO: Criar relação com a classe Usuario
        public string LoginUsuarioCadastro { get; set; }
        public Nullable<DateTime> DataCancelamento { get; set; }
        //TODO: Criar relação com a classe Usuario
        public string LoginUsuarioCancelamento { get; set; }
        public string MotivoCancelamento { get; set; }
        public virtual ICollection<CotacaoItem> ListaItens { get; set; }

        public Cotacao()
        {
            this.ListaItens = new HashSet<CotacaoItem>();
        }
    }
}