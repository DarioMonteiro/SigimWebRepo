using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Entity.CredCob;

namespace GIR.Sigim.Domain.Entity.Comercial
{
    public class VendaSerie : BaseEntity
    {
        public int ContratoId { get; set; }
        //public ContratoComercial Contrato { get; set; }
        public int NumeroSerie { get; set; }
        public Byte NomeSerie { get; set; }
        public Decimal CapitalSerie { get; set; }
        public String TipoCapital { get; set; }
        public String FormaFinanciamento { get; set; }
        public String Periodicidade { get; set; }
        public Decimal PercentualJurosSerie { get; set; }
        public Int16 NumeroParcelas { get; set; }
        public DateTime DataPrimeiroVencimento { get; set; }
        public Decimal ValorParcela { get; set; }
        public int? IndiceCorrecaoId { get; set; }
        //public IndiceFinanceiro IndiceCorrecao { get; set; }
        public Nullable<DateTime> DataBaseIndiceCorrecao { get; set; }
        public Nullable<Decimal> CotacaoIndiceCorrecao { get; set; }
        public int? IndiceAtrasoCorrecaoId { get; set; }
        //public IndiceFinanceiro IndiceAtrasoCorrecao { get; set; }
        public String CobrancaResiduo { get; set; }
        public Nullable<DateTime> DataBaseAniversarioCobrancaResiduo { get; set; }
        public int? DefasagemDia { get; set; }
        public int? DefasagemMes { get; set; }
        public Nullable<DateTime> DataBaseTabelaVenda { get; set; }
        public Nullable<Decimal> PercentualJurosDefasagem { get; set; }
        public String TipoJurosDefasagem { get; set; }
        public int? IndiceReajusteId { get; set; }
        //public IndiceFinanceiro IndiceReajuste { get; set; }
        public Nullable<DateTime> DataBaseIndiceReajuste { get; set; }
        public Nullable<Decimal> CotacaoIndiceReajuste { get; set; }
        public Nullable<DateTime> DataBaseJuros { get; set; }
        public Nullable<DateTime> DataProximoAniversario { get; set; }
        public Nullable<DateTime> DataUltimoAniversario { get; set; }
        public int? RenegociacaoId { get; set; }
        public Nullable<DateTime> DataCancelamentoRenegociacao { get; set; }
        public Int16 DefasagemMesIndiceCorrecao { get; set; }

        public ICollection<TituloCredCob> ListaTituloCredCob { get; set; }

        public VendaSerie()
        {
            this.ListaTituloCredCob = new HashSet<TituloCredCob>();
        }
    }
}
