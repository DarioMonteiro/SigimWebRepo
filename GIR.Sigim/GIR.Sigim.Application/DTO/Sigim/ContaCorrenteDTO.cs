using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class ContaCorrenteDTO : BaseDTO
    {
               
        public int? BancoId { get; set; }
        public BancoDTO Banco { get; set; }

        public int? AgenciaId { get; set; }
        public AgenciaDTO Agencia { get; set; }

        [StringLength(15, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Conta")]
        public string ContaCodigo { get; set; }

        [StringLength(10, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "DV")]
        public string DVConta { get; set; }

        [StringLength(30, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Código empresa")]
        public string CodigoEmpresa { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Nome cedente")]
        public string NomeCedente { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "CNPJ cedente")]
        public string CNPJCedente { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Complemento")]
        public string Complemento { get; set; }

        //public byte? Tipo { get; set; }

        [StringLength(1, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Situação")]
        public string Situacao { get; set; }       
        //public bool Inativo
        //{
        //    get { return Situacao == "I"; }
        //    set { Situacao = value ? "I" : "A"; }
        //}
        
    }
}