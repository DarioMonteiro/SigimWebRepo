using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.DTO.Sac
{
    public class ParametrosSacDTO : BaseDTO
    {
        [Display(Name = "Empresa")]
        public int? ClienteId { get; set; }
        public ClienteFornecedorDTO Cliente { get; set; }

        [Required]
        [Display(Name = "Mascara")]
        public string Mascara { get; set; }

        [Display(Name = "Ícone para relatórios")]
        public byte[] IconeRelatorio { get; set; }
        public bool RemoverImagem { get; set; }

        [Required]
        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Prazo de avaliação")]
        public int? PrazoAvaliacao { get; set; }

        [Required]
        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Prazo de conclusão")]
        public int? PrazoConclusao { get; set; }

        [Required]
        [Display(Name = "Email de envio")]
        public string EmailEnvio { get; set; }

        [Required]
        [Display(Name = "Senha")]
        public string senhaEnvio { get; set; }

        [Required]
        [Display(Name = "Porta")]
        public string PortaEnvio { get; set; }

        [Required]
        [Display(Name = "SMTP")]
        public string ServidorEnvio { get; set; }

        [StringLength(1000, ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Mensagem automatica Sac Web")]
        public string CorpoMensagemAutomaticaSacweb { get; set; }

        [Display(Name = "Habilita SSL")]
        public bool HabilitaSSL { get; set; }

        public List<ParametrosEmailSacDTO> ListaParametrosEmailSac { get; set; }

        public ParametrosSacDTO()
        {
            this.ListaParametrosEmailSac = new List<ParametrosEmailSacDTO>();
        }
     }
}