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
        public ContratoLicitacao Licitacao { get; set; }
        public ClienteFornecedor Contratante { get; set; }
        public ClienteFornecedor Contratado { get; set; }
        public ClienteFornecedor Interveniente { get; set; }
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


    }
}
