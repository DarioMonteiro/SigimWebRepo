using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Comercial
{
    public class Venda : BaseEntity
    {
        public int ContratoId { get; set; }
        public ContratoComercial Contrato { get; set; }
        public Nullable<DateTime> DataVenda { get; set; }
        public int TabelaVendaId { get; set; }
        public TabelaVenda TabelaVenda { get; set; }
        public Decimal PrecoTabela { get; set; }
        public Decimal? ValorDesconto { get; set; }
        public Decimal PrecoPraticado { get; set; }
        public String Condicao { get; set; }
        public Decimal PrecoContrato { get; set; }
        public int? IndiceFinanceiroId { get; set; }
        public IndiceFinanceiro IndiceFinanceiro { get; set; }
        public Decimal CotacaoIndiceFinanceiro { get; set; }
        public DateTime DataBaseIndiceFinanceiro { get; set; }
        public Nullable<DateTime> DataAssinaturaAgenda { get; set; }
        public String HoraAssinaturaAgenda { get; set; }
        public Nullable<DateTime> DataAssinatura { get; set; }
        public String HoraAssinatura { get; set; }
        public String NumeroCartorio { get; set; }
        public String NumeroLivroCartorio { get; set; }
        public String NumeroFolhaLivroCartorio { get; set; }
        public String FormaVenda { get; set; }
        public String FormaContrato { get; set; }
        public int? ContaCorrenteId { get; set; }
        public ContaCorrente ContaCorrente { get; set; }
        public Nullable<DateTime> DataCadastramento { get; set; }
        public Nullable<DateTime> DataQuitacao { get; set; }
        public Nullable<DateTime> DataCancelamento { get; set; }
        public Nullable<Boolean> Aprovado { get; set; }
        public String UsuarioAprovacao { get; set; }
        public Nullable<DateTime> DataAprovacao { get; set; }
        public Decimal? PrecoBaseComissao { get; set; }
        public int? MatrizId { get; set; }
        public int? CorretorMatrizId { get; set; }
        public Decimal? ValorTotalComissao { get; set; }

        //public virtual ICollection<VendaParticipante> ListaVendaParticipante { get; set; }

        public Venda()
        {
            //this.ListaVendaParticipante = new HashSet<VendaParticipante>();
        }
        
    }
}
