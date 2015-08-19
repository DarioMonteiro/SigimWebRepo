using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Entity.Sac;
using GIR.Sigim.Application.Adapter;

namespace GIR.Sigim.Application.DTO.Sac
{
    public class ParametrosEmailSacDTO : BaseDTO
    {
        [Required]
        [Display(Name = "Tipo")]
        public SituacaoSolicitacaoSac? Tipo { get; set; }

        public string DescricaoTipo
        {
            get { return this.Tipo.ObterDescricao(); }
        }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Setor")]
        public int? SetorId { get; set; }
        public SetorDTO Setor { get; set; }

        [Display(Name = "Anexo")]
        public bool Anexo { get; set; }

        public string DescricaoAnexo
        {
            get { return Anexo == true ? "Sim" : "Não"; }
        }

        [Display(Name = "Parametros")]
        public int? ParametrosId { get; set; }
        public ParametrosSacDTO ParametrosSac { get; set; }
     }
}