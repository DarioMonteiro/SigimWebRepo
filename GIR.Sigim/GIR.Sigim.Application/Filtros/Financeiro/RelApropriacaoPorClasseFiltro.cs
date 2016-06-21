using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Application.Filtros.Financeiro
{
    public class RelApropriacaoPorClasseFiltro : BaseFiltro
    {
        [DataType(DataType.Date, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "InformeDataValida")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data inicial")]
        public Nullable<DateTime> DataInicial { get; set; }

        [DataType(DataType.Date, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "InformeDataValida")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data final")]
        public Nullable<DateTime> DataFinal { get; set; }

        public CentroCustoDTO CentroCusto { get; set; }

        public int? OpcoesRelatorio { get; set; }

        [Display(Name = "Por competência")]
        public bool EhTipoPesquisaPorCompetencia { get; set; }

        [Display(Name = "Por emissão documento")]
        public bool EhTipoPesquisaPorEmissao { get; set; }

        public List<ClasseDTO> ListaClasseDespesa { get; set; }

        [Display(Name = "Movimentos de débito ")]
        public bool EhMovimentoDebito { get; set; }

        [Display(Name = "Movimentos de débito caixa")]
        public bool EhMovimentoDebitoCaixa { get; set; }

        [Display(Name = "Provisionado")]
        public bool EhSituacaoAPagarProvisionado { get; set; }

        [Display(Name = "Aguardando Liberação")]
        public bool EhSituacaoAPagarAguardandoLiberacao { get; set; }

        [Display(Name = "Liberado")]
        public bool EhSituacaoAPagarLiberado { get; set; }

        [Display(Name = "Emitido")]
        public bool EhSituacaoAPagarEmitido { get; set; }

        [Display(Name = "Pago")]
        public bool EhSituacaoAPagarPago { get; set; }

        [Display(Name = "Baixado")]
        public bool EhSituacaoAPagarBaixado { get; set; }

        [Display(Name = "Cancelado")]
        public bool EhSituacaoAPagarCancelado { get; set; }

        public List<ClasseDTO> ListaClasseReceita { get; set; }

        [Display(Name = "Movimentos de crédito")]
        public bool EhMovimentoCredito { get; set; }

        [Display(Name = "Movimentos de crédito caixa")]
        public bool EhMovimentoCreditoCaixa { get; set; }

        [Display(Name = "Movimentos de crédito cobrança")]
        public bool EhMovimentoCreditoCobranca { get; set; }

        [Display(Name = "Provisionado")]
        public bool EhSituacaoAReceberProvisionado { get; set; }

        [Display(Name = "A faturar")]
        public bool EhSituacaoAReceberAFatura { get; set; }

        [Display(Name = "Faturado")]
        public bool EhSituacaoAReceberFaturado { get; set; }

        [Display(Name = "Pré-datado")]
        public bool EhSituacaoAReceberPreDatado { get; set; }

        [Display(Name = "Recebido")]
        public bool EhSituacaoAReceberRecebido { get; set; }

        [Display(Name = "Quitado")]
        public bool EhSituacaoAReceberQuitado { get; set; }

        [Display(Name = "Cancelado")]
        public bool EhSituacaoAReceberCancelado { get; set; }

        public RelApropriacaoPorClasseFiltro()
        {
            OpcoesRelatorio = 1;

            ListaClasseDespesa = new List<ClasseDTO>();
            ListaClasseReceita = new List<ClasseDTO>();

        }
    }
}
