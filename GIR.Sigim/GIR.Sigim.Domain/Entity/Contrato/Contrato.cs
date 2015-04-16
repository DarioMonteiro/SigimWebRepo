using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro; 
using GIR.Sigim.Domain.Entity.Sigim;   

namespace GIR.Sigim.Domain.Entity.Contrato
{
    public class Contrato : BaseEntity
    {
        public string CodigoCentroCusto { get; set; }
        public virtual CentroCusto CentroCusto { get; set; }
        public int? LicitacaoId { get; set; }
        public Licitacao Licitacao { get; set; }
        public int ContratanteId { get; set; }
        public ClienteFornecedor Contratante { get; set; }
        public int ContratadoId { get; set; }
        public ClienteFornecedor Contratado { get; set; }
        public int? IntervenienteId { get; set; } 
        public ClienteFornecedor Interveniente { get; set; }
        public int ContratoDescricaoId { get; set; }
        public LicitacaoDescricao ContratoDescricao { get; set; }
        public SituacaoContrato Situacao { get; set; }
        public Nullable<DateTime> DataAssinatura { get; set; }
        public string DocumentoOrigem { get; set; }
        public string NumeroEmpenho { get; set; }
        public decimal? ValorContrato { get; set; }
        public DateTime DataCadastro { get; set; }
        public string UsuarioCadastro { get; set; }
        public Nullable<DateTime> DataCancela { get; set; }
        public string UsuarioCancela { get; set; }
        public string MotivoCancela { get; set; }
        public int TipoContrato { get; set; }

        public ICollection<ContratoRetificacao> ListaContratoRetificacao { get; set; }
        //public ICollection<ContratoRetificacaoItem> ListaContratoRetificacaoItem { get; set; }


        public Contrato()
        {
            this.Situacao = SituacaoContrato.Minuta;
            this.ListaContratoRetificacao = new HashSet<ContratoRetificacao>();
            //this.ListaContratoRetificacaoItem = new HashSet<ContratoRetificacaoItem>(); 
        }

    }
}
