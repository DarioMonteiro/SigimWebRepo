using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class ApropriacaoClasseCCRelatorio : BaseEntity
    {
        public string TipoClasseCC { get; set; }
        public Classe Classe { get; set; }
        public CentroCusto CentroCusto { get; set; }
        public string CorrentistaConta { get; set; }
        public string SiglaDocumento { get; set; }
        public string Documento { get; set; }
        public string Identificacao { get; set;}
        public decimal ValorApropriado { get; set; }
        public decimal ValorAcumulado { get; set; }
        public string Situacao { get; set; }
        public string TipoCodigo { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataEmissaoDocumento { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataPagamento { get; set; }
        public DateTime DataBaixa { get; set; }
        public string  MesAno { get; set; }
        public string NomeCliente { get; set; }
    }
}
