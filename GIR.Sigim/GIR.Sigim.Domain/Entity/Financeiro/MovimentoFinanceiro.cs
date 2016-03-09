using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Entity.CredCob;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class MovimentoFinanceiro : BaseEntity
    {
        public int TipoMovimentoId { get; set; }
        public TipoMovimento TipoMovimento { get; set; }
        public DateTime DataMovimento { get; set; }
        public int? ContaCorrenteId { get; set; }
        public ContaCorrente ContaCorrente { get; set; }
        public int? CaixaId { get; set; }
        public Caixa Caixa { get; set; }
        public string Referencia { get; set; }
        public string Documento { get; set; }
        public Decimal Valor { get; set; }
        public string Situacao { get; set; }
        public String UsuarioLancamento { get; set; }
        public Nullable<DateTime> DataLancamento { get; set; }
        public String UsuarioConferencia { get; set; }
        public Nullable<DateTime> DataConferencia { get; set; }
        public String UsuarioApropriacao { get; set; }
        public Nullable<DateTime> DataApropriacao { get; set; }
        public int? MovimentoPaiId { get; set; }
        public MovimentoFinanceiro MovimentoPai { get; set; }
        public int? MovimentoOposto { get; set; }
        public int? BorderoTransferencia { get; set; }

        public virtual ICollection<MovimentoFinanceiro> ListaFilhos { get; set; }
        public virtual ICollection<Apropriacao> ListaApropriacao { get; set; }
        public ICollection<TituloMovimento> ListaTituloMovimento { get; set; }


        public MovimentoFinanceiro()
        {
            this.ListaFilhos = new HashSet<MovimentoFinanceiro>();
            this.ListaApropriacao = new HashSet<Apropriacao>();
            this.ListaTituloMovimento = new HashSet<TituloMovimento>();
        }
    }
}
