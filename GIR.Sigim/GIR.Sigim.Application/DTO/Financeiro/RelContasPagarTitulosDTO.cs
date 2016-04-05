using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.DTO.Financeiro
{
    public class RelContasPagarTitulosDTO 
    {
        public int TituloId { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataEmissaoDocumento { get; set; }
        public string DocumentoCompleto { get; set; }
        public decimal ValorTitulo { get; set; }
        public decimal ValorLiquido { get; set; }
        public int? ClienteId { get; set; }
        public string NomeCliente { get; set; }
        public string Identificacao { get; set; }
        public string FormaPagamentoDescricao { get; set; }
        public string DocumentoPagamento { get; set; }
        public string AgenciaContaCorrente { get; set; }
        public string TipoCompromissoDescricao { get; set; }
        public string SituacaoTituloDescricao { get; set; }
        public Nullable<DateTime> DataSelecao { get; set; }
        public Nullable<DateTime> DataEmissao { get; set; }
        public Nullable<DateTime> DataPagamento { get; set; }
        public Nullable<DateTime> DataBaixa { get; set; }
        public string MotivoCancelamentoDescricao { get; set; }
        public string CPFCNPJ { get; set; }
        public string LoginUsuarioCadastro { get; set; }
        public Nullable<DateTime> DataCadastro { get; set; }
        public decimal ValorApropriado { get; set; }
        public string CodigoClasse { get; set; }
        public string CodigoDescricaoClasse { get; set; }
        public string CodigoCentroCusto { get; set; }
        public string CodigoDescricaoCentroCusto { get; set; }
    }
}
