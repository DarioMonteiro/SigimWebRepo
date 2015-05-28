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
        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Prazo de avaliação")]
        public short? PrazoAvaliacao { get; set; }

        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Prazo de conclusão")]
        public short? PrazoConclusao { get; set; }

        [Display(Name = "Empresa")]
        public int? ClienteId { get; set; }
        public ClienteFornecedorDTO Cliente { get; set; }

        [Display(Name = "Mascara")]
        public string Mascara { get; set; }

        [Display(Name = "Email")]
        public string EmailEnvia { get; set; }

        [Display(Name = "Senha")]
        public string senhaEnvio { get; set; }

        [Display(Name = "Porta")]
        public string PortaEnvio { get; set; }

        [Display(Name = "SMTP")]
        public string ServidorEnvio { get; set; }

        [Display(Name = "Ícone para relatórios")]
        public byte[] IconeRelatorio { get; set; }
        public bool RemoverImagem { get; set; }

        [Display(Name = "Mensagem automatica Sac Web")]
        public string CorpoMensagemAutomaticaSacweb { get; set; }

        [Display(Name = "Habilita SSL")]
        public bool HabilitaSSL { get; set; }

     }
}