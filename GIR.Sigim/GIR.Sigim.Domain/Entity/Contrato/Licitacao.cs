using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Contrato
{
    public class Licitacao : BaseEntity 
    {
        public string CodigoCentroCusto { get; set; }
        public virtual CentroCusto CentroCusto { get; set; }
        public int? LicitacaoCronogramaId { get; set; }
        public LicitacaoCronograma LicitacaoCronograma { get; set; }
        public DateTime DataLicitacao { get; set; }
        public SituacaoLicitacao Situacao { get; set; }
        public string Observacao { get; set; }
        public int? ClienteFornecedorId { get; set; }
        public ClienteFornecedor ClienteFornecedor { get; set; }
        public Nullable<DateTime> DataLimiteEmail { get; set; }
        public string ReferenciaDigital { get; set; }
        public DateTime DataCadastro { get; set; }
        public string UsuarioCadastro { get; set; }
        public Nullable<DateTime> DataCancela { get; set; }
        public string UsuarioCancela { get; set; }
        public string MotivoCancela { get; set; }

        public ICollection<Contrato> ListaContrato { get; set; }

        public Licitacao()
        {
            this.ListaContrato = new HashSet<Contrato>();
        }
    }
}
