using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class BancoDTO : BaseDTO
    {
        [StringLength(50, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        public string Situacao { get; set; }
        public bool Ativo 
        {
            get { return Situacao == "A"; }
            set { Situacao = value ? "A" : "I"; }
        }
        [Display(Name = "Rem. cobrança")]
        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        public int? NumeroRemessa { get; set; }

        [Display(Name = "Interface eletrônica")]
        public bool InterfaceEletronica { get; set; }
        public string InterfaceEletronicaDescricao
        {
            get { return InterfaceEletronica == true ? "Sim" : "Não"; }
        }

        [Display(Name = "Rem. pagamento")]
        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        public int? NumeroRemessaPagamento { get; set; }       

        
        public List<AgenciaDTO> ListaAgencia { get; set; }

        public BancoDTO()
        {
           this.ListaAgencia = new List<AgenciaDTO>(); 
        }
    }
}