using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.DTO.Financeiro
{
    public class ParametrosFinanceiroDTO : BaseDTO 
    {

        [Display(Name = "Empresa")]
        public int? ClienteId { get; set; }
        public ClienteFornecedorDTO Cliente { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Responsável")]
        public string Responsavel { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Praça de pagamento")]
        public string PracaPagamento { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Licenca")]
        public string Licenca { get; set; }

        [StringLength(18, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "CentroCusto")]
        public string CentroCusto { get; set; }

        [StringLength(18, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Classe")]
        public string Classe { get; set; }

        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Situação default do título a pagar")]
        public short? SituacaoDefaultPagar { get; set; }

        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Situação default do título a receber")]
        public short? SituacaoDefaultReceber { get; set; }

        [Display(Name = "Gera automática de título de imposto")]
        public bool GeraTituloImposto { get; set; }

        [Display(Name = "Leitora de código de barras")]
        public bool LeitoraCodigoBarras { get; set; }

        [RegularExpression(@"^\d+(.\d+){0,1}$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Tolerância para recebimento")]
        public decimal? ToleranciaRecebimento { get; set; }

        [Display(Name = "Bloqueia edição total de títulos")]
        public bool? InterfaceBloqueioTotal { get; set; }
              
        [Display(Name = "Bloqueia edição parcial de títulos")]
        public bool? InterfaceBloqueioParcial { get; set; }

        [Display(Name = "Permite alteração somente de apropriação")]
        public bool? InterfacePermiteApropriacao { get; set; }

        [Display(Name = "Interface contábil")]
        public bool InterfaceContabil { get; set; }
        
        [Display(Name = "Não permitir cadastro de títulos com apropriação superior a 100%")]
        public bool PercentualApropriacao { get; set; }

        [Display(Name = "Ícone para relatórios")]
        public byte[] IconeRelatorio { get; set; }

        public bool RemoverImagem { get; set; }

        [Display(Name = "Confere nota fiscal (EM)")]
        public bool ConfereNFOC { get; set; }

        [Display(Name = "Confere nota fiscal (Contrato)")]
        public bool ConfereNFCT { get; set; }

        [Display(Name = "Não permitir alterar impostos")]
        public bool ImpostoAutomatico { get; set; }

        [Display(Name = "Não pagar títulos com valor apropriado diferente do valor pago")]
        public bool ValorLiquidoApropriado { get; set; }

        [Display(Name = "Não pagar títulos em conta corrente não associada ao c. custo")]
        public bool ContaCorrenteCentroCusto { get; set; }

        public short Interface { get; set; }
      
    }
}
