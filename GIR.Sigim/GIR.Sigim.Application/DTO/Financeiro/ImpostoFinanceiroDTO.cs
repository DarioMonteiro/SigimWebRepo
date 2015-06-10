using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.DTO.Financeiro
{
    public class ImpostoFinanceiroDTO : BaseDTO 
    {
        [Required]
        [StringLength(50, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Sigla")]
        public string Sigla { get; set; }

        [Required]
        [StringLength(50, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Required]
        [RegularExpression(@"^\d+(.\d+){0,1}$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Alíquota")]
        public decimal Aliquota { get; set; }

        [Display(Name = "Retido")]
        public bool Retido { get; set; }

        public string RetidoDescricao
        { get { return Retido == true ? "Sim" : "Não"; } }

        [Display(Name = "Indireto")]
        public bool Indireto { get; set; }

        public string IndiretoDescricao
        { get { return Indireto == true ? "Sim" : "Não"; } }

        [Display(Name = "Pagamento Eletrônico")]
        public bool PagamentoEletronico { get; set; }

        public string PagamentoEletronicoDescricao
        { get { return PagamentoEletronico == true ? "Sim" : "Não"; } }

        [Display(Name = "Correntista")]
        public int? ClienteId { get; set; }
        public ClienteFornecedorDTO Cliente { get; set; }

        [Display(Name = "Compromisso")]
        public int? TipoCompromissoId { get; set; }
        public TipoCompromissoDTO TipoCompromisso { get; set; }

        [StringLength(20, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Conta contábil")]
        public string ContaContabil { get; set; }

        [Display(Name = "Periodicidade")]
        public int? Periodicidade { get; set; }

        [Display(Name = "Fim de semana")]
        public int? FimDeSemana { get; set; }

        [Display(Name = "Fato gerador")]
        public int? FatoGerador { get; set; }

        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Dia do vencimento")]
        public Int16? DiaVencimento { get; set; }

    }
}
