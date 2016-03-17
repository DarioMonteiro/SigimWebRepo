using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Application.Filtros.Financeiro
{
    public class RelContasAPagarTitulosFiltro : BaseFiltro
    {
        [DataType(DataType.Date, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "InformeDataValida")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data inicial")]
        public Nullable<DateTime> DataInicial { get; set; }

        [DataType(DataType.Date, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "InformeDataValida")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data final")]
        public Nullable<DateTime> DataFinal { get; set; }

        public ClienteFornecedorDTO ClienteFornecedor { get; set; }

        [Display(Name = "Identificação")]
        [StringLength(70, ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        public string Identificacao { get; set; }

        [Display(Name = "Documento")]
        [StringLength(10, ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        public string Documento { get; set; }

        [Display(Name = "Compromisso")]
        public int? TipoCompromissoId { get; set; }
        public TipoCompromissoDTO TipoCompromisso { get; set; }

        [Display(Name = "Forma de pagamento")]
        public string FormaPagamentoCodigo { get; set; }
        public FormaPagamentoDTO FormaPagamento { get; set; }

        public ClasseDTO Classe { get; set; }
        public CentroCustoDTO CentroCusto { get; set; }

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

        public short? VisualizarClientePor { get; set; }

        [Display(Name = "Banco")]
        public int? BancoId { get; set; }
        public BancoDTO Banco { get; set; }

        [Display(Name = "Ag / Cc")]
        public int? ContaCorrenteId { get; set; }
        public ContaCorrenteDTO ContaCorrente { get; set; }

    }
}
