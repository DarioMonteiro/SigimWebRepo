using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public ICollection<ContratoRetencao> ListaContratoRetencao { get; set; }

        public ContratoRetificacaoItemMedicao()
        {
            this.ListaContratoRetencao = new HashSet<ContratoRetencao>();
        }

        private bool ValidaData(string Data)
        {
            DateTime dataValida;

            if (!DateTime.TryParse(Data, out dataValida))
            {
                return false;
            }

            return true;
        }

        protected int ComparaDatas(string Data1, string Data2)
        {
            int result = 0;
            DateTime data1, data2;

            if (!DateTime.TryParseExact(Data1, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out data1))
            {
                return 1;
            }

            if (!DateTime.TryParseExact(Data2, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out data2))
            {
                return -1;
            }

            if (data1 > data2) result = -1;
            else if (data1 < data2) result = 1;

            return result;
        }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool condicao = false;

            //if (Situacao > SituacaoMedicao.AguardandoLiberacao)
            //{
            //    yield return new ValidationResult(string.Format(Resource.Contrato.ErrorMessages.SituacaoNaoPermitida, "Situação da medição"));
            //}

            condicao = ((DataMedicao == null) || (DataMedicao == DateTime.MinValue));
            if (condicao)
            {
                yield return new ValidationResult(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Data emissão"));
            }
            else
            {
                if (!ValidaData(DataMedicao.ToShortDateString()))
                {
                    yield return new ValidationResult(string.Format(Resource.Sigim.ErrorMessages.CampoInvalido, "Data emissão"));
                }
            }

            condicao = ((DataVencimento == null) || (DataVencimento == DateTime.MinValue));
            if (condicao)
            {
                yield return new ValidationResult(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Data vencimento"));
            }
            else
            {
                if (!ValidaData(DataVencimento.ToShortDateString()))
                {
                    yield return new ValidationResult(string.Format(Resource.Sigim.ErrorMessages.CampoInvalido, "Data vencimento"));
                }
            }

            if ((DataEmissao != null) && (DataVencimento != null))
            {
                if (ComparaDatas(DataEmissao.ToShortDateString(), DataVencimento.ToShortDateString()) < 0)
                {
                    yield return new ValidationResult(string.Format(Resource.Sigim.ErrorMessages.DataMaiorQue, "Data emissão", "Data vencimento"));
                }
            }

            if (TipoDocumentoId == 0)
            {
                yield return new ValidationResult(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Tipo"));
            }

            if (string.IsNullOrEmpty(NumeroDocumento))
            {
                yield return new ValidationResult(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Documento"));
            }

            condicao = ((DataMedicao == null) || (DataMedicao == DateTime.MinValue));
            if (condicao)
            {
                yield return new ValidationResult(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Data medição"));
            }
            else
            {
                if (!ValidaData(DataMedicao.ToShortDateString()))
                {
                    yield return new ValidationResult(string.Format(Resource.Sigim.ErrorMessages.CampoInvalido, "Data medição"));
                }
            }

            if ((DataMedicao != null) && (DataVencimento != null))
            {
                if (ComparaDatas(DataMedicao.ToShortDateString(), DataVencimento.ToShortDateString()) < 0)
                {
                    yield return new ValidationResult(string.Format(Resource.Sigim.ErrorMessages.DataMaiorQue, "Data medição", "Data vencimento"));
                }
            }

            if (Quantidade == 0)
            {
                yield return new ValidationResult(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Quantidade medição atual"));
            }

            //if (ContratoRetificacaoItem != null)
            //{
            //    if (ContratoRetificacaoItem.NaturezaItem == NaturezaItem.PrecoGlobal)
            //    {
            //        if (Valor == 0)
            //        {
            //            yield return new ValidationResult(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Valor medição atual"));
            //        }

            //        decimal valorTotalMedido = Contrato.ObterValorTotalMedido(SequencialItem, SequencialCronograma);
            //        decimal valorItem = 0;
            //        if (ContratoRetificacaoItem.ValorItem.HasValue)
            //        {
            //            valorItem = ContratoRetificacaoItem.ValorItem.Value;
            //        }

            //        decimal valorPendente = valorItem - valorTotalMedido;

            //        if (Valor > valorPendente)
            //        {
            //            yield return new ValidationResult(string.Format(Resource.Sigim.ErrorMessages.ValorMaiorQue, "Valor medição atual", "Valor pendente"));
            //        }
            //    }
            //    else if (ContratoRetificacaoItem.NaturezaItem == NaturezaItem.PrecoUnitario)
            //    {

            //        decimal quantidadeTotalMedida = Contrato.ObterQuantidadeTotalMedida(SequencialItem, SequencialCronograma);
            //        decimal quantidadePendente = ContratoRetificacaoItem.Quantidade - quantidadeTotalMedida;

            //        if (Quantidade > quantidadePendente)
            //        {
            //            yield return new ValidationResult(string.Format(Resource.Sigim.ErrorMessages.ValorMaiorQue, "Quantidade medição atual", "Quantidade pendente"));
            //        }
            //    }
            //}

            if (Desconto > 0)
            {
                if (string.IsNullOrEmpty(MotivoDesconto))
                {
                    yield return new ValidationResult(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Motivo desconto"));
                }
                if (Desconto > Valor)
                {
                    yield return new ValidationResult(string.Format(Resource.Sigim.ErrorMessages.ValorMaiorQue, "Desconto", "Valor medição atual"));
                }
            }

            if (!string.IsNullOrEmpty(MotivoDesconto))
            {
                if (Desconto == 0)
                {
                    yield return new ValidationResult(string.Format(Resource.Sigim.ErrorMessages.CampoObrigatorio, "Desconto"));
                }
            }
        }
    }
}
