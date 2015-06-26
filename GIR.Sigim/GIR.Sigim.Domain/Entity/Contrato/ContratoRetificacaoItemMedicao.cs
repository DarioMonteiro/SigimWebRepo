using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Contrato
{
    public class ContratoRetificacaoItemMedicao : BaseEntity
    {
        public int? ContratoId { get; set; }
        public Contrato Contrato { get; set; }
        public int ContratoRetificacaoId { get; set; }
        public ContratoRetificacao ContratoRetificacao { get; set; }
        public int ContratoRetificacaoItemId { get; set; }
        public ContratoRetificacaoItem ContratoRetificacaoItem { get; set; }
        public int SequencialItem { get; set; }
        public int ContratoRetificacaoItemCronogramaId { get; set; }
        public ContratoRetificacaoItemCronograma ContratoRetificacaoItemCronograma { get; set; }
        public int SequencialCronograma { get; set; }
        public SituacaoMedicao Situacao { get; set; }
        public int TipoDocumentoId { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal Quantidade { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataMedicao { get; set; }
        public string UsuarioMedicao { get; set; }
        public int? MultiFornecedorId { get; set; }
        public ClienteFornecedor MultiFornecedor { get; set; }
        public string Observacao { get; set; }
        public int? TituloPagarId { get; set; }
        public TituloPagar TituloPagar { get; set; }
        public int? TituloReceberId { get; set; }
        public TituloReceber TituloReceber { get; set; }
        public Nullable<DateTime> DataLiberacao { get; set; }
        public string UsuarioLiberacao { get; set; }
        public decimal? ValorRetido { get; set; }
        public string TipoCompraCodigo { get; set; }
        public TipoCompra TipoCompra { get; set; }
        public int? CifFobId { get; set; }
        public CifFob CifFob { get; set; }
        public string NaturezaOperacaoCodigo { get; set; }
        public NaturezaOperacao NaturezaOperacao { get; set;}
        public int? SerieNFId { get; set; }
        public SerieNF SerieNF { get; set; }
        public string CSTCodigo { get; set; }
        public CST CST { get; set; }
        public string CodigoContribuicaoCodigo { get; set; }
        public CodigoContribuicao CodigoContribuicao { get; set; }
        public string CodigoBarras { get; set; }
        public decimal? BaseIPI { get; set; }
        public decimal? PercentualIpi { get; set; }
        public decimal? BaseIcms { get; set; }
        public decimal? PercentualIcms { get; set; }
        public decimal? BaseIcmsSt { get; set; }
        public decimal? PercentualIcmsSt { get; set; }
        public bool? Conferido { get; set; }
        public string UsuarioConferencia { get; set; }
        public Nullable<DateTime> DataConferencia { get; set; }
        public Nullable<DateTime> DataCadastro { get; set; }
        public decimal? Desconto { get; set; }
        public string MotivoDesconto { get; set; }

    }
}
