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
        [StringLength(3, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Código do banco")]
        public string BancoCodigo { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        public string Situacao { get; set; }
        public bool Ativo 
        {
            get { return Situacao == "A"; }
            set { Situacao = value ? "A" : "I"; }
        }
        public int? NumeroRemessa { get; set; }                

        public bool InterfaceEletronica { get; set; }
        public string InterfaceEletronicaDescricao
        {
            get { return InterfaceEletronica == true ? "Sim" : "Não"; }
        }

        public int? NumeroRemessaPagamento { get; set; }       

        
        public ICollection<AgenciaDTO> ListaAgencia { get; set; }

        public BancoDTO()
        {
           this.ListaAgencia = new HashSet<AgenciaDTO>(); 
        }
    }
}